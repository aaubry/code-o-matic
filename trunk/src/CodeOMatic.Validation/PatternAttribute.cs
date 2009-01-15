using System;
using PostSharp.Extensibility;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using PostSharp.CodeModel;

namespace CodeOMatic.Validation
{
    /// <summary>
    /// Validates that a parameter respects the regular expression.
    /// </summary>
    [Serializable]
	public sealed class PatternAttribute : SpecificExceptionParameterValidatorAttribute
    {
		private readonly string expression;

		/// <summary>
		/// Gets or sets the expression to be used as validation.
		/// </summary>
		/// <value>The validation expression.</value>
		public string Expression
		{
			get
			{
				return expression;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PatternAttribute"/> class.
		/// </summary>
		/// <param name="expression">The expression.</param>
		public PatternAttribute(string expression)
		{
			this.expression = expression;
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

			Type parameterType = parameter.ParameterType.GetSystemType(null, null);
			bool isString = parameterType == typeof(string);

			if (!isString)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"PatternAttribute_TypeNotSupported",
					string.Format(CultureInfo.InvariantCulture, "The type '{0}' is not a string to perform the expression validation on.", parameterType.Name),
					GetType().FullName
				));
			}

			if (string.IsNullOrEmpty(expression))
			{
				messages.Write(new Message(
					SeverityType.Error,
					"PatternAttribute_PatternNotNullOrEmpty",
					"The expression pattern must not be null or empty.",
					GetType().FullName
				));
			}
		}

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
				string realValue = (string)value;
				Match match = Regex.Match(realValue, expression);
				bool isValid = ((match.Success && (match.Index == 0)));
				if (!isValid)
				{
					ValidationFailed("The parameter did not match the defined pattern.", value, parameterName);
				}
			}
		}
	}
}