using System;
using System.Collections.Generic;

namespace CodeOMatic.Validation.CompileTime.Parser
{
	/// <summary>
	/// Parser for test files
	/// </summary>
	internal partial class SelectorParser
	{
		private IList<MemberSelector> selectors;

		public IEnumerable<MemberSelector> Parse()
		{
			ParseInternal();
			return selectors;
		}

		private void StartParsingSelectors()
		{
			selectors = new List<MemberSelector>();
		}

		private void AddSelector(IEnumerable<SelectorPart> parts)
		{
			selectors.Add(new MemberSelector(parts));
		}
	}
}