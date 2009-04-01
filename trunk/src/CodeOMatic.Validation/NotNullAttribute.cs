using System;
using System.Globalization;
using PostSharp.Extensibility;
using PostSharp.CodeModel;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Validates that a parameter is not null.
	/// </summary>
	/// <remarks>
	/// This attribute supports any reference type as well as <see cref="Nullable{T}"/>.
	/// </remarks>
	[Serializable]
	public sealed class NotNullAttribute : SpecificExceptionParameterValidatorAttribute
	{
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

			bool isNullable =
				!memberType.IsValueType ||
				(memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>));

			if (!isNullable)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"NotNullAttribute_TypeNotNullable",
					string.Format(CultureInfo.InvariantCulture, "The type '{0}' is not nullable.", memberType.Name),
					GetType().FullName
				));
			}
		}

		/// <summary>
		/// Validates the specified value.
		/// </summary>
		/// <param name="target">The object on which the method is being called.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public override void Validate(object target, object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}
	}
}
