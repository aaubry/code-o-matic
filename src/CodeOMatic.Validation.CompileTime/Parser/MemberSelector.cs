using System;
using System.Collections.Generic;

namespace CodeOMatic.Validation.CompileTime.Parser
{
	internal class MemberSelector
	{
		private readonly IEnumerable<SelectorPart> parts;
		
		public MemberSelector(IEnumerable<SelectorPart> parts)
		{
			this.parts = parts;
		}

		public IEnumerable<SelectorPart> Parts
		{
			get
			{
				return parts;
			}
		}

		public void Accept(ISelectorVisitor visitor)
		{
			foreach(var part in parts)
			{
				part.Accept(visitor);
			}
		}
	}
}