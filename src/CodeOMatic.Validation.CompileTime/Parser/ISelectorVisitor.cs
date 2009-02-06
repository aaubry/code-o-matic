using System;

namespace CodeOMatic.Validation.CompileTime.Parser
{
	/// <summary>
	/// Part of the implementation of the Visitor pattern for <see cref="SelectorPart"/>
	/// </summary>
	internal interface ISelectorVisitor
	{
		/// <summary>
		/// Visits the specified part.
		/// </summary>
		/// <param name="part">The part.</param>
		void Visit(MemberSelectorPart part);

		/// <summary>
		/// Visits the specified part.
		/// </summary>
		/// <param name="part">The part.</param>
		void Visit(IterationSelectorPart part);
	}
}