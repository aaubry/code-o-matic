using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using PostSharp.Extensibility;
using PostSharp.CodeModel;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Implements validation by calling a user-provided method.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Parameter)]
	public sealed class CustomValidatorAttribute : Attribute, IParameterValidator, IMethodValidator
	{
		private readonly string method;

		/// <summary>
		/// Gets the name of the method to use for the validation.
		/// </summary>
		public string Method
		{
			get
			{
				return method;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomValidatorAttribute"/> class.
		/// </summary>
		/// <param name="method">The name of the method to use for the validation.</param>
		/// <remarks>
		/// The <paramref name="method"/> parameter must be the name of a method of the current
		/// class. Its signature depends on whether the attribute has been applied to a method or
		/// to a parameter. If it is applied to a method, its signature must be the following:
		/// <code>void Validate(IDictionary&lt;string, object&gt; parameters)</code>. Otherwise,
		/// its signature must be the following:
		/// <code>void Validate(object parameterValue, string parameterName)</code>.
		/// </remarks>
		public CustomValidatorAttribute(string method)
		{
			if(method == null)
			{
				throw new ArgumentNullException("method");
			}

			this.method = method;
		}

		#region IParameterValidator Members
		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on a specific parameter.
		/// </summary>
		/// <param name="parameter">The parameter on which the attribute is applied.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		public void CompileTimeValidate(ParameterDeclaration parameter, IMessageSink messages)
		{
			MethodBase validator = GetValidationMethod(parameter);
			if (validator == null)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"CustomAttribute_MethodNotFound",
					string.Format(CultureInfo.InvariantCulture, "Could not find a suitable method called '{0}'.", method),
					GetType().FullName
				));
			}
		}

		[NonSerialized]
		private MethodBase validationMethod;

		/// <summary>
		/// Gets the validation method.
		/// </summary>
		/// <param name="parameter">The parameter to be validated.</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public MethodBase GetValidationMethod(ParameterDeclaration parameter)
		{
			if (validationMethod == null)
			{
				Type parameterType = parameter.ParameterType.GetSystemType(null, null);
				Type validatedType = parameter.Parent.DeclaringType.GetSystemType(null, null);

				const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				MethodInfo[] methods = validatedType.GetMethods(bindingFlags);
				foreach(var methodInfo in methods)
				{
					if(methodInfo.Name == method && methodInfo.ReturnType == typeof(void))
					{
						if(ParameterKindDetector.GetParameterKinds(methodInfo, parameterType, validatedType) != null)
						{
							validationMethod = methodInfo;
							break;
						}
					}
				}
			}
			return validationMethod;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is persistent.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is persistent; otherwise, <c>false</c>.
		/// </value>
		public bool IsPersistent
		{
			get
			{
				return false;
			}
		}
		#endregion

		#region IMethodValidator Members
		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on a specific method.
		/// </summary>
		/// <param name="method">The method on which the attribute is applied.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		public void CompileTimeValidate(MethodDefDeclaration method, IMessageSink messages)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Validates the parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="parameters">The parameter values.</param>
		public void Validate(object target, IDictionary<string, object> parameters)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}