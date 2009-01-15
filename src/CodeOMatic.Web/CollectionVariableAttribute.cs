using System;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
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

		///
		public override bool CompileTimeValidate(MethodBase method)
		{
			bool foundProperty = false;
			foreach (var property in method.DeclaringType.UnderlyingSystemType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (method.Name == property.GetGetMethod().Name)
				{
					isGetter = true;
					foundProperty = true;
				}
				else if (method.Name == property.GetSetMethod().Name)
				{
					isGetter = false;
					foundProperty = true;
				}

				if (foundProperty)
				{
					if (defaultValue != null && defaultValue.GetType() != property.PropertyType)
					{
						MessageSource.MessageSink.Write(new Message(
							SeverityType.Error,
							"CollectionVariableAttribute_DefaultValueHasTheWrongType",
							"The type of the default value does not match the type of the property.",
							GetType().FullName
						));
					}

					key = key ?? string.Format(
						CultureInfo.InvariantCulture,
						"{0}.{1}",
						property.DeclaringType.FullName,
						property.Name
					);

					CompileTimeValidate(property, method, isGetter);

					break;
				}
			}

			if (!foundProperty)
			{
				MessageSource.MessageSink.Write(new Message(
					SeverityType.Error,
					"CollectionVariableAttribute_MethodDoesNotBelongToProperty",
					string.Format(CultureInfo.InvariantCulture, "The method '{0}' does not belong to a property.", method.Name),
					GetType().FullName
				));
			}

			return base.CompileTimeValidate(method);
		}

		/// <summary>
		/// Validates the usage of the attribute on a specific property.
		/// </summary>
		/// <param name="propertyInfo">The property.</param>
		/// <param name="method">The method.</param>
		/// <param name="isGetMethod">if set to <c>true</c> [is get method].</param>
		protected virtual void CompileTimeValidate(PropertyInfo propertyInfo, MethodBase method, bool isGetMethod)
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

			if (defaultValue == null)
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
			if (HttpContext.Current == null)
			{
				throw new InvalidOperationException("This property should only be accessed in the context of an HTTP request.");
			}

			HttpSessionState session = HttpContext.Current.Session;
			if (session == null)
			{
				throw new InvalidOperationException("There is no session.");
			}

			Debug.Assert(eventArgs.Instance != null);
			if (isGetter)
			{
				eventArgs.ReturnValue = GetValue(eventArgs.Instance, defaultValue);
			}
			else
			{
				SetValue(eventArgs.Instance, eventArgs.GetReadOnlyArgumentArray()[0], defaultValue);
			}
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		/// <returns></returns>
		protected abstract object GetValue(object target, object defaultValue);

		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="target">The object on which the property should be set.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		protected abstract void SetValue(object target, object value, object defaultValue);
	}
}
