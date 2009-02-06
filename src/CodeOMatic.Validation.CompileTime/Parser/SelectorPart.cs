using System;

namespace CodeOMatic.Validation.CompileTime.Parser
{
	internal abstract class SelectorPart
	{
		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public abstract void Accept(ISelectorVisitor visitor);
	}
}