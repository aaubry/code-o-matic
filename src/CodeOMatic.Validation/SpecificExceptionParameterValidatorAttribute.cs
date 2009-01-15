using System;
using System.Reflection;
using PostSharp.CodeModel;
using PostSharp.Extensibility;
using System.Globalization;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Base attribute for validating method parameters that throws a user-specified exception when
	/// the validation fails.
	/// </summary>
	[Serializable]
	public abstract class SpecificExceptionParameterValidatorAttribute : ParameterValidatorAttribute
	{
		private Type exception;

		/// <summary>
		/// Gets the type of the exception that is thrown when the validation fails.
		/// </summary>
		public Type Exception
		{
			get
			{
				return exception;
			}
			set
			{
				exception = value;
			}
		}

		private string message;

		/// <summary>
		/// Gets or sets the exception message.
		/// </summary>
		public string Message
		{
			get
			{
				return message;
			}
			set
			{
				message = value;
			}
		}

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
		public override void CompileTimeValidate(ParameterDeclaration parameter, IMessageSink messages)
		{
			base.CompileTimeValidate(parameter, messages);

			if(exception != null)
			{
				if(!ValidateExceptionTypeIsException(messages))
				{
					return;
				}
				if(!ValidateExceptionTypeCanBeInstantiated(messages))
				{
					return;
				}
			}
		}

		private bool ValidateExceptionTypeIsException(IMessageSink messages)
		{
			bool isValid = typeof(Exception).IsAssignableFrom(exception);
			if(!isValid)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"SpecificExceptionParameterValidatorAttribute_InvalidExceptionType",
					string.Format(CultureInfo.InvariantCulture, "The type '{0}' is not an exception type.", exception.Name),
					GetType().FullName
				));
			}
			return isValid;
		}

		private bool ValidateExceptionTypeCanBeInstantiated(IMessageSink messages)
		{
			ConstructorInfo constructor = exception.GetConstructor(
				BindingFlags.Public | BindingFlags.Instance,
				null,
				new[] {typeof(string)},
				null
			);

			bool isValid = constructor != null;
			if(!isValid)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"SpecificExceptionParameterValidatorAttribute_ExceptionTypeCanotBeInstantiated",
					string.Format(CultureInfo.InvariantCulture, "The exception '{0}' does not have a public constructor that takes a string as parameter.", exception.Name),
					GetType().FullName
				));
			}
			return isValid;
		}

		/// <summary>
		/// Throws an exception according to the validator's parameters.
		/// </summary>
		/// <param name="validationMessage">The validation message.</param>
		/// <param name="parameterName">Name of the parameter that is being validated.</param>
		/// <param name="parameterValue">The value of the parameter.</param>
		protected void ValidationFailed(string validationMessage, object parameterValue, string parameterName)
		{
			string errorMessage = message ?? validationMessage;
			if (exception == null)
			{
				throw CreateDefaultException(errorMessage, parameterName, parameterValue);
			}
			else
			{
				throw (Exception)Activator.CreateInstance(exception, errorMessage);
			}
		}

		/// <summary>
		/// Creates the default exception for the validator.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="parameterName">Name of the parameter that is being validated.</param>
		/// <param name="parameterValue">The value of the parameter.</param>
		/// <returns></returns>
		protected virtual Exception CreateDefaultException(string errorMessage, string parameterName, object parameterValue)
		{
			return new ArgumentException(errorMessage, parameterName);
		}
	}
}