using System;

namespace CodeOMatic.Validation.CompileTime.Parser
{
	internal class IterationSelectorPart : SelectorPart
	{
		private IterationSelectorPart()
		{	
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public override void Accept(ISelectorVisitor visitor)
		{
			visitor.Visit(this);
		}

		public static readonly IterationSelectorPart Instance = new IterationSelectorPart();
	}
}