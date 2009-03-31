using System;

namespace CodeOMatic.Validation.CompileTime.Parser
{
	internal class MemberSelectorPart : SelectorPart
	{
		private readonly string memberName;

		public string MemberName
		{
			get
			{
				return memberName;
			}
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public override void Accept(ISelectorVisitor visitor)
		{
			visitor.Visit(this);
		}

		public MemberSelectorPart(string memberName)
		{
			this.memberName = memberName;
		}
	}
}