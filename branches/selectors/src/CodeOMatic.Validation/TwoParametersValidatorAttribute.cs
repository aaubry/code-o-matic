using System;
using System.Collections.Generic;
using CodeOMatic.Validation.Core;
using PostSharp.CodeModel;
using PostSharp.Extensibility;
using System.Globalization;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Base class for validating a pair of attributes,
	/// </summary>
	[Serializable]
	public abstract class TwoParametersValidatorAttribute : SpecificExceptionMethodValidatorAttribute
	{
		private readonly string firstParameterName;

		/// <summary>
		/// Gets the name of the first parameter.
		/// </summary>
		public string FirstParameter
		{
			get
			{
				return firstParameterName;
			}
		}

		private readonly string secondParameterName;

		/// <summary>
		/// Gets the name of the second parameter.
		/// </summary>
		public string SecondParameter
		{
			get
			{
				return secondParameterName;
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="TwoParametersValidatorAttribute"/> class.
		/// </summary>
		/// <param name="firstParameter">The name of the first parameter.</param>
		/// <param name="secondParameter">The name of the second parameter.</param>
		protected TwoParametersValidatorAttribute(string firstParameter, string secondParameter)
		{
			firstParameterName = firstParameter;
			secondParameterName = secondParameter;
		}
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
		public override void CompileTimeValidate(MethodDefDeclaration method, IMessageSink messages)
		{
			base.CompileTimeValidate(method, messages);

			ParameterDeclaration firstParameter = null;
			ParameterDeclaration secondParameter = null;

			foreach(var parameter in method.Parameters)
			{
				if(parameter.Name == firstParameterName)
				{
					firstParameter = parameter;
				}
				if(parameter.Name == secondParameterName)
				{
					secondParameter = parameter;
				}
			}

			if(firstParameter == null)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"TwoParametersValidatorAttribute_ParameterNotFound",
					string.Format(CultureInfo.InvariantCulture,
						"The parameter '{0}' does not exist in method '{1}'.",
						firstParameterName,
						method.Name),
					GetType().FullName
				));
			}

			if(secondParameter == null)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"TwoParametersValidatorAttribute_ParameterNotFound",
					string.Format(CultureInfo.InvariantCulture,
						"The parameter '{0}' does not exist in method '{1}'.",
						secondParameterName,
						method.Name),
					GetType().FullName
				));
			}

			if(firstParameter != null && secondParameter != null)
			{
				if(firstParameterName == secondParameterName)
				{
					{
						messages.Write(new Message(
							SeverityType.Error,
							"TwoParametersValidatorAttribute_CantSpecifyTheSameParameter",
							"The same name has been specified for both parameters.",
							GetType().FullName
						));
					}
				}
				else
				{
					CompileTimeValidateParameters(method, messages, firstParameter, secondParameter);
				}
			}
		}

		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on specific method parameters.
		/// </summary>
		/// <param name="firstParameter">The first parameter.</param>
		/// <param name="secondParameter">The second parameter.</param>
		/// <param name="method">The method on which the attribute is applied.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		protected virtual void CompileTimeValidateParameters(MethodDefDeclaration method, IMessageSink messages, ParameterDeclaration firstParameter, ParameterDeclaration secondParameter)
		{
		}

		/// <summary>
		/// Validates the parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="parameters">The parameter values.</param>
		public override void Validate(object target, IDictionary<string, object> parameters)
		{
			Validate(target, parameters[firstParameterName], parameters[secondParameterName]);
		}

		/// <summary>
		/// Validates the parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="first">The first parameter.</param>
		/// <param name="second">The second parameter.</param>
		protected abstract void Validate(object target, object first, object second);
	}
}