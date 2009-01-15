using System;
using PostSharp.CodeModel;
using PostSharp.Extensibility;
using System.Reflection;

namespace CodeOMatic.Validation.Core
{
	/// <summary>
	/// Interface that attributes must implement for validating a single method parameter.
	/// </summary>
	[CLSCompliant(false)]
	public interface IParameterValidator : IValidator
	{
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
		void CompileTimeValidate(ParameterDeclaration parameter, IMessageSink messages);

		/// <summary>
		/// Gets the validation method.
		/// </summary>
		/// <param name="parameter">The parameter to be validated.</param>
		/// <returns></returns>
		MethodBase GetValidationMethod(ParameterDeclaration parameter);
	}
}