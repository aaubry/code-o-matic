using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;

namespace CodeOMatic.Core
{
	/// <summary>
	/// Converts strings to objects.
	/// </summary>
	public static class StringConverter
	{
		private static readonly Regex stringParser = new Regex(@"^\[(?<TypeName>(?>[^\[\]]+|\[(?<Depth>)|\](?<-Depth>))*(?(Depth)(?!)))\](?<Separator>\s?)(?<Value>.*)$", RegexOptions.Compiled | RegexOptions.Singleline);

		private delegate object StringParsingFunction(string value);

		private static readonly IDictionary<Type, StringParsingFunction> knownParsingFunctions = new Dictionary<Type, StringParsingFunction>
		{
			{ typeof(bool), value => (object)bool.Parse(value) },
			{ typeof(byte), value => (object)byte.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(sbyte), value => (object)sbyte.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(char), value => (object)char.Parse(value) },
			{ typeof(decimal), value => (object)decimal.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(double), value => (object)double.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(float), value => (object)float.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(int), value => (object)int.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(uint), value => (object)uint.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(long), value => (object)long.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(ulong), value => (object)ulong.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(short), value => (object)short.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(ushort), value => (object)ushort.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(DateTime), value => (object)DateTime.Parse(value, CultureInfo.InvariantCulture) },
			{ typeof(string), value => value },
		};

		private static readonly IDictionary<string, Type> knownTypes = new Dictionary<string, Type>
		{
			{ "bool", typeof(bool) },
			{ "byte", typeof(byte) },
			{ "sbyte", typeof(sbyte) },
			{ "char", typeof(char) },
			{ "decimal", typeof(decimal) },
			{ "double", typeof(double) },
			{ "float", typeof(float) },
			{ "int", typeof(int) },
			{ "uint", typeof(uint) },
			{ "long", typeof(long) },
			{ "ulong", typeof(ulong) },
			{ "short", typeof(short) },
			{ "ushort", typeof(ushort) },
			{ "date", typeof(DateTime) },
			{ "string", typeof(string) },
			{ "", typeof(string) },
		};

		/// <summary>
		/// Converts the specified string to an object.
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <returns></returns>
		/// <remarks>
		/// In order to be converted to object, the string must have the following format:
		/// <pre>
		/// '[' TypeName '] ' Value
		/// </pre>
		/// (Notice the space between the closing bracket and the value.)
		/// TypeName is the name of the .NET type that will parse the value, and Value is the value to be parsed.
		/// 
		/// TypeName can be one of the following:
		/// <list>
		///	<item>The fully qualified name of the type (e.g. System.Int32, mscorlib).</item>
		///	<item>The full name of the type (e.g. System.Int32).</item>
		///	<item>The name of a type from the System namespace (e.g. Int32).</item>
		///	<item>An alias from the following list:
		///		<list>
		///		<item>bool</item>
		///		<item>byte</item>
		///		<item>sbyte</item>
		///		<item>char</item>
		///		<item>decimal</item>
		///		<item>double</item>
		///		<item>float</item>
		///		<item>int</item>
		///		<item>uint</item>
		///		<item>long</item>
		///		<item>ulong</item>
		///		<item>short</item>
		///		<item>ushort</item>
		///		<item>date</item>
		///		<item>string</item>
		///		</list>
		/// </item>
		/// <item>An empty string. In that case, the String type is assumed.</item>
		/// </list>
		/// 
		/// The type must have a publc static method with one of the following signatures:
		/// <pre>
		/// public static T Parse(string value, IFormatProvider formatProvider);
		/// public static T Parse(string value);
		/// </pre>
		/// The type T can be any type. The methods are tested in the order indicated here. The value parameter takes the
		/// Value part of the string. The formatProvider parameter is always passed <see cref="CultureInfo.InvariantCulture"/>.
		/// 
		/// In order to specify a string that starts with '[' and that should not be converted, prepend "[] " to it.
		/// </remarks>
		public static object Convert(string value)
		{
			if(value == null)
			{
				return null;
			}

			Match match = stringParser.Match(value);
			if(match.Success)
			{
				if(match.Groups["Separator"].Length != 1)
				{
					throw new ArgumentException("For clarity, there should be a space after the ']' that closes the type name. Simply add a space after the last ']' to fix this error. If you don't want this string to be converted, append '[] ' at the beginning.");
				}

				Type type = GetType(match.Groups["TypeName"].Value);
				StringParsingFunction parser = GetParsingFunction(type);
				return parser(match.Groups["Value"].Value);
			}
			else if(value.StartsWith("[", StringComparison.Ordinal))
			{
				throw new ArgumentException("The specified value starts with '[' but could not be parsed. This usually indicates an error in the string. If you don't want this string to be converted, append '[] ' at the beginning.");
			}
			else
			{
				return value;
			}
		}

		/// <summary>
		/// If the <paramref name="value"/> parameter is a string, converts it to object using the <see cref="Convert"/> class.
		/// Otherwise, returns the value of the parameter.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static object ConvertIfString(object value)
		{
			string text = value as string;
			return text != null ? Convert(text) : value;
		}

		private static StringParsingFunction GetParsingFunction(Type type)
		{
			StringParsingFunction parser;
			if(!knownParsingFunctions.TryGetValue(type, out parser))
			{
				lock(knownParsingFunctions)
				{
					if (!knownParsingFunctions.TryGetValue(type, out parser))
					{
						parser = DiscoverParsingFunction(type);
						knownParsingFunctions.Add(type, parser);
					}
				}
			}
			return parser;
		}

		private static StringParsingFunction DiscoverParsingFunction(Type type)
		{
			return
				DiscoverParsingFunction(type, "Parse", typeof(ParseStringFormatFunction<>), typeof(string), typeof(IFormatProvider)) ??
				DiscoverParsingFunction(type, "Parse", typeof(ParseStringFunction<>), typeof(string));
		}

		private static StringParsingFunction DiscoverParsingFunction(Type type, string methodName, Type parseFunctionDelegateType, params Type[] parameterTypes)
		{
			var parsingFunction = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, parameterTypes, null);
			if (parsingFunction != null)
			{
				Type specificDelegateType = parseFunctionDelegateType.MakeGenericType(parsingFunction.ReturnType);

				object functionDelegate = Delegate.CreateDelegate(specificDelegateType, parsingFunction);

				MethodInfo genericAdapterCreator = typeof(StringConverter).GetMethod("Create" + parseFunctionDelegateType.Name.Replace("`1", "") + "Adapter", BindingFlags.Static | BindingFlags.NonPublic);
				MethodInfo adapterCreator = genericAdapterCreator.MakeGenericMethod(parsingFunction.ReturnType);
				return (StringParsingFunction)adapterCreator.Invoke(null, new[] { functionDelegate });
			}

			return null;
		}

		private delegate T ParseStringFunction<T>(string value);

		private static StringParsingFunction CreateParseStringFunctionAdapter<T>(ParseStringFunction<T> function)
		{
			return value => (object)function(value);
		}

		private delegate T ParseStringFormatFunction<T>(string value, IFormatProvider formatProvider);

		private static StringParsingFunction CreateParseStringFormatFunctionAdapter<T>(ParseStringFormatFunction<T> function)
		{
			return value => (object)function(value, CultureInfo.InvariantCulture);
		}

		private static Type GetType(string typeName)
		{
			Type type;

			if (!knownTypes.TryGetValue(typeName, out type))
			{
				lock(knownTypes)
				{
					if (!knownTypes.TryGetValue(typeName, out type))
					{
						type = DiscoverType(typeName);
						knownTypes.Add(typeName, type);
					}
				}
			}

			return type;
		}

		private static Type DiscoverType(string typeName)
		{
			// Try an exact match
			Type type = Type.GetType(typeName, false);
			if(type != null)
			{
				return type;
			}

			// Search in every loaded assembly for an exact match
			foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}

			// Search for types in the System namespace
			type = Type.GetType("System." + typeName, false);
			if (type != null)
			{
				return type;
			}

			throw new TypeLoadException(string.Format(CultureInfo.InvariantCulture, "Could not load type '{0}'.", typeName));
		}
	}
}