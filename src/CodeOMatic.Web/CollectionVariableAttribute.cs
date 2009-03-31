using System;
using System.Globalization;
using System.Reflection;
using PostSharp.Extensibility;
using PostSharp.Laos;
using System.Diagnostics;

namespace CodeOMatic.Web
{
	/// <summary>
	/// Base class for implementing properties based on dictionaries.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class CollectionVariableAttribute : ImplementMethodAspect
	{
		private string key;
		private object defaultValue;

		private string defaultValueMethod;

		private delegate object GetDefaultMethod(object target);
		private delegate TReturn GetDefaultInstanceMethod<TArgument, TReturn>(TArgument target);
		private delegate TReturn GetDefaultStaticMethod<TReturn>();

		private static GetDefaultMethod MakeGetDefaultInstanceMethod<TArgument, TReturn>(GetDefaultInstanceMethod<TArgument, TReturn> specificMethod)
		{
			return target => specificMethod((TArgument)target);
		}

		private static GetDefaultMethod MakeGetDefaultStaticMethod<TReturn>(GetDefaultStaticMethod<TReturn> specificMethod)
		{
			return target => specificMethod();
		}

		[NonSerialized]
		private GetDefaultMethod defaultValueMethodStub;

		private bool isGetter;


		/// <summary>
		/// Gets or sets the name of the variable.
		/// </summary>
		public string Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		/// <summary>
		/// Gets or sets the default value of the property.
		/// </summary>
		/// <value>The default value.</value>
		public object DefaultValue
		{
			get
			{
				return defaultValue;
			}
			set
			{
				defaultValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the method that provides the default value of the property.
		/// </summary>
		public string DefaultValueMethod
		{
			get
			{
				return defaultValueMethod;
			}
			set
			{
				defaultValueMethod = value;
			}
		}

		/// <summary>
		/// Allows derived classes to perform compile-time initializations.
		/// </summary>
		/// <param name="propertyType">Type of the property.</param>
		protected virtual void CompileTimeInitialize(Type propertyType)
		{
			// Nothing to be done
		}

		///
		public override bool CompileTimeValidate(MethodBase method)
		{
			ParameterInfo[] parameters = method.GetParameters();

			if (defaultValue != null && defaultValueMethod != null)
			{
				MessageSource.MessageSink.Write(new Message(
					SeverityType.Error,
					"CollectionVariableAttribute_DefaultValueAndDefaultValueMethodAreBothSpecified",
					"DefaultValue and DefaultValueMethod cannot be both specified.",
					GetType().FullName
				));
			}

			isGetter = parameters.Length == 0;

			string propertyName = method.Name.Replace(isGetter ? "get_" : "set_", "");
			key = key ?? string.Format(
				CultureInfo.InvariantCulture,
				"{0}.{1}",
				method.DeclaringType.FullName,
				propertyName
			);

			if (!isGetter)
			{
				Type propertyType = parameters[0].ParameterType;
				if (defaultValue != null)
				{
					if (defaultValue.GetType() != propertyType)
					{
						MessageSource.MessageSink.Write(new Message(
							SeverityType.Error,
							"CollectionVariableAttribute_DefaultValueHasTheWrongType",
							"The type of the default value does not match the type of the property.",
							GetType().FullName
						));
					}
				}
				else if (defaultValueMethod != null)
				{
					MethodInfo defaultMethod = method.DeclaringType.UnderlyingSystemType.GetMethod(
						defaultValueMethod,
						BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
						null,
						Type.EmptyTypes,
						null
					);

					if (defaultMethod == null)
					{
						MessageSource.MessageSink.Write(new Message(
							SeverityType.Error,
							"CollectionVariableAttribute_DefaultValueMethodNotFound",
							string.Format(CultureInfo.InvariantCulture, "A method '{0}' without parameters could not be found on the type.", defaultValueMethod),
							GetType().FullName
						));
					}
					else if (defaultMethod.ReturnType != propertyType.UnderlyingSystemType)
					{
						MessageSource.MessageSink.Write(new Message(
							SeverityType.Error,
							"CollectionVariableAttribute_DefaultValueMethodHasTheWrongType",
							"The type of the default value method does not match the type of the property.",
							GetType().FullName
						));
					}
				}

				CompileTimeValidate(method, propertyType, propertyName);
			}

			return base.CompileTimeValidate(method);
		}

		/// <summary>
		/// Validates the usage of the attribute on a specific property.
		/// </summary>
		/// <param name="setter">The setter method of the property.</param>
		/// <param name="propertyType">Type of the property.</param>
		/// <param name="propertyName">Name of the property.</param>
		protected virtual void CompileTimeValidate(MethodBase setter, Type propertyType, string propertyName)
		{
		}

		// This method is called using reflection.
		// ReSharper disable UnusedPrivateMember
		private static T GetDefault<T>()
		{
			return default(T);
		}
		// ReSharper restore UnusedPrivateMember

		///
		public override void RuntimeInitialize(MethodBase method)
		{
			base.RuntimeInitialize(method);

			if (defaultValueMethod != null && isGetter)
			{
				MethodInfo defaultMethod = method.DeclaringType.GetMethod(
					defaultValueMethod,
					BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
					null,
					Type.EmptyTypes,
					null
				);

				Type returnType = ((MethodInfo)method).ReturnType;

				if (defaultMethod.IsStatic)
				{
					Type specificDelegateType = typeof(GetDefaultStaticMethod<>).MakeGenericType(returnType);
					object genericDelegate = Delegate.CreateDelegate(specificDelegateType, defaultMethod);

					MethodInfo genericMakeGetDefaultMethod = typeof(CollectionVariableAttribute).GetMethod(
						"MakeGetDefaultStaticMethod",
						BindingFlags.Static | BindingFlags.NonPublic
					);

					MethodInfo makeGetDefaultMethod = genericMakeGetDefaultMethod.MakeGenericMethod(returnType);
					defaultValueMethodStub = (GetDefaultMethod)makeGetDefaultMethod.Invoke(null, new[] { genericDelegate });
				}
				else
				{
					Type specificDelegateType = typeof(GetDefaultInstanceMethod<,>).MakeGenericType(method.DeclaringType, returnType);
					object genericDelegate = Delegate.CreateDelegate(specificDelegateType, defaultMethod);

					MethodInfo genericMakeGetDefaultMethod = typeof(CollectionVariableAttribute).GetMethod(
						"MakeGetDefaultInstanceMethod",
						BindingFlags.Static | BindingFlags.NonPublic
					);

					MethodInfo makeGetDefaultMethod = genericMakeGetDefaultMethod.MakeGenericMethod(method.DeclaringType, returnType);
					defaultValueMethodStub = (GetDefaultMethod)makeGetDefaultMethod.Invoke(null, new[] { genericDelegate });
				}
			}
			else if (defaultValue == null)
			{
				Type propertyType = isGetter ? ((MethodInfo)method).ReturnType : method.GetParameters()[0].ParameterType;

				// If no default value is specified, get the default value for that type.
				MethodInfo openGetDefault = typeof(CollectionVariableAttribute).GetMethod("GetDefault", BindingFlags.Static | BindingFlags.NonPublic);
				MethodInfo closedGetDefault = openGetDefault.MakeGenericMethod(propertyType);
				defaultValue = closedGetDefault.Invoke(null, null);
			}
		}

		///
		public override void OnExecution(MethodExecutionEventArgs eventArgs)
		{
			Debug.Assert(eventArgs.Instance != null);
			if (isGetter)
			{
				eventArgs.ReturnValue = GetValue(eventArgs.Instance);
			}
			else
			{
				SetValue(eventArgs.Instance, eventArgs.GetReadOnlyArgumentArray()[0], defaultValue);
			}
		}

		/// <summary>
		/// Gets the default value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <returns></returns>
		protected object GetDefaultValue(object target)
		{
			if (!isGetter)
			{
				throw new NotSupportedException("The GetDefaultValue method can only be invoked inside the GetValue method.");
			}
			return defaultValueMethodStub != null ? defaultValueMethodStub(target) : defaultValue;
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <returns></returns>
		protected abstract object GetValue(object target);

		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="target">The object on which the property should be set.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		protected abstract void SetValue(object target, object value, object defaultValue);
	}
}