using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using PostSharp.CodeModel;
using PostSharp.Extensibility;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Validates that a parameter is not empty.
	/// </summary>
	/// <remarks>
	/// This attributes supports the following types: <see cref="String"/>, <see cref="ICollection"/> and <see cref="ICollection{T}"/>
	/// </remarks>
	[Serializable]
	public sealed class NotEmptyAttribute : SpecificExceptionParameterValidatorAttribute
	{
		private enum ParameterKind
		{
			None,
			String,
			ICollection,
			ICollectionOfT
		}

		private ParameterKind parameterKind;

		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on a specific parameter.
		/// </summary>
		/// <param name="parameter">The parameter on which the attribute is applied.</param>
		/// <param name="memberType">The type that will be validated by the attribute.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		public override void CompileTimeValidate(ParameterDeclaration parameter, Type memberType, IMessageSink messages)
		{
			base.CompileTimeValidate(parameter, memberType, messages);

			if (memberType == typeof(string))
			{
				parameterKind = ParameterKind.String;
				return;
			}

			if (typeof(ICollection).IsAssignableFrom(memberType))
			{
				parameterKind = ParameterKind.ICollection;
				return;
			}

			if (TypeIsICollectionOfT(memberType))
			{
				parameterKind = ParameterKind.ICollectionOfT;
				getCount = CreateGetCountDelegate(memberType);
				return;
			}

			messages.Write(new Message(
				SeverityType.Error,
				"NotEmptyAttribute_TypeNotSupported",
				string.Format(CultureInfo.InvariantCulture, "The type '{0}' is not supported.", memberType.Name),
				GetType().FullName
			));
		}

		private static bool TypeIsICollectionOfT(Type type)
		{
			if (type.IsInterface && InterfaceIsICollectionOfT(type))
			{
				return true;
			}

			foreach (var interfaceType in type.GetInterfaces())
			{
				if (InterfaceIsICollectionOfT(interfaceType))
				{
					return true;
				}
			}
			return false;
		}

		private static bool InterfaceIsICollectionOfT(Type type)
		{
			return
				type.Assembly == typeof(ICollection<>).Assembly &&
				type.Namespace == typeof(ICollection<>).Namespace &&
				type.Name == typeof(ICollection<>).Name;
		}

		private delegate int GetCountDelegate(object collection);

		// This method is called through reflection.
		// ReSharper disable UnusedPrivateMember
		private static GetCountDelegate MakeGetCountDelegate<T, U>() where T : ICollection<U>
		{
			return collection => ((T)collection).Count;
		}
		// ReSharper restore UnusedPrivateMember

		private static GetCountDelegate CreateGetCountDelegate(Type interfaceType)
		{
			Type itemType = interfaceType.GetGenericArguments()[0];

			MethodInfo openMakeGetCountDelegate = typeof(NotEmptyAttribute).GetMethod("MakeGetCountDelegate", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo makeGetCountDelegate = openMakeGetCountDelegate.MakeGenericMethod(interfaceType, itemType);

			return (GetCountDelegate)makeGetCountDelegate.Invoke(null, new object[0]);
		}

		private GetCountDelegate getCount;

		/// <summary>
		/// Validates the parameter.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public override void Validate(object target, object value, string parameterName)
		{
			if (value != null)
			{
				switch (parameterKind)
				{
					case ParameterKind.String:
						ValidateString((string)value, parameterName);
						break;

					case ParameterKind.ICollection:
						ValidateCollection((ICollection)value, parameterName);
						break;

					case ParameterKind.ICollectionOfT:
						ValidateCollectionOfT(value, parameterName);
						break;

					default:
						throw new InvalidOperationException("An invalid ParameterKind has been detected.");
				}
			}
		}

		private void ValidateCollectionOfT(object parameterValue, string parameterName)
		{
			//PropertyInfo property = typeof(ICollection<>).GetProperty("Count", BindingFlags.Instance | BindingFlags.Public);
			//int count = (int)property.GetValue(parameterValue, null
			if (getCount(parameterValue) == 0)
			{
				InvokeValidationFailed(parameterName, parameterValue);
			}
		}

		private void ValidateCollection(ICollection parameterValue, string parameterName)
		{
			if (parameterValue.Count == 0)
			{
				InvokeValidationFailed(parameterName, parameterValue);
			}
		}

		private void ValidateString(string parameterValue, string parameterName)
		{
			if (parameterValue.Length == 0)
			{
				InvokeValidationFailed(parameterName, parameterValue);
			}
		}

		private void InvokeValidationFailed(string parameterName, object parameterValue)
		{
			ValidationFailed(
				string.Format(CultureInfo.InvariantCulture, "The parameter '{0}' is empty.", parameterName),
				parameterValue,
				parameterName
			);
		}
	}
}