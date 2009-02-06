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
		/// <param name="memberType">The type that will be validated by the attribute.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		void CompileTimeValidate(ParameterDeclaration parameter, Type memberType, IMessageSink messages);

		/// <summary>
		/// Gets the validation method.
		/// </summary>
		/// <param name="parameter">The parameter on which the attribute is applied.</param>
		/// <param name="memberType">The type that will be validated by the attribute.</param>
		/// <returns></returns>
		MethodBase GetValidationMethod(ParameterDeclaration parameter, Type memberType);

		/// <summary>
		/// Gets a string that specifies on what elements of the parameter the attribute should be aplied.
		/// </summary>
		/// <remarks>
		/// The selectors have the following syntax:
		/// <pre>
		/// Selectors =	Selector { selectorseparator Selector }.
		/// Selector = [Member] [Iteration] { ',' Member [Iteration] }.
		/// Iteration = '*'.
		/// Member = valid_C#_identifier.
		/// </pre>
		/// Each selector specifies the path to the members on which the attribute should applied.
		/// Members are C# identifiers that represent the name of a proerty on the current object.
		/// </remarks>
		/// <example>
		/// <code>
		/// public void SendEmail(
		///		[NotNull(Selector = ", Name, Email")]			// Checks that:
		///		Address from,									//  - the parameter itself is not null
		///														//  - the Name and Email properties are not null
		/// 
		///		[NotNull(Selector = ", *, *.Name, *.Email")]	// Checks that:
		///     IEnumerable&lt;Address&gt; to					//  - the parameter itself is not null
		///														//  - each element is not null
		///														//  - the Name and Email properties of each element are not null
		/// )
		/// {
		///		// ...
		/// }
		/// </code>
		/// </example>
		string Selectors
		{
			get;
		}
	}
}