using System;
using MbUnit.Framework;
using CodeOMatic.Validation.CompileTime.Parser;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class ScopeParserTests
	{
		private class ToStringSelectorVisitor : ISelectorVisitor
		{
			private readonly StringBuilder stringRepresentation = new StringBuilder();

			public override string ToString()
			{
				return stringRepresentation.ToString();
			}

			#region ISelectorVisitor Members
			public void Visit(MemberSelectorPart part)
			{
				if(stringRepresentation.Length > 0)
				{
					stringRepresentation.Append('~');
				}
				stringRepresentation.Append(part.MemberName);
			}

			public void Visit(IterationSelectorPart part)
			{
				stringRepresentation.Append('+');
			}
			#endregion
		}

		[Row("PropertyName")]
		[Row("PropertyName.ChildProperty")]
		[Row("*")]
		[Row("*.ChildProperty")]
		[Row("Enumerable*.ChildProperty")]
		[Row("Enumerable*.ChildProperty,AnotherProperty")]
		[Row("From, To		  , To*.Name,     To*.Email")]
		[Row(@"
			From,
			To,
			To*.Name,
			To*.Email
		")]
		[Row(",*")]
		[RowTest]
		public void SelectorTests(string selectors)
		{
			Console.WriteLine(selectors);

			SelectorParser parser = new SelectorParser(new SelectorScanner(new MemoryStream(Encoding.UTF8.GetBytes(selectors))));

			List<string> parsedSelectors = new List<string>();
			foreach(var selector in parser.Parse())
			{
				ToStringSelectorVisitor visitor = new ToStringSelectorVisitor();
				foreach(var part in selector.Parts)
				{
					part.Accept(visitor);
				}
				parsedSelectors.Add(visitor.ToString());
				Console.WriteLine("  {0}", visitor);
			}

			int index = 0;
			foreach(var selector in selectors.Split(','))
			{
				Assert.AreEqual(selector.Trim(), parsedSelectors[index++].Replace('~', '.').Replace('+', '*'));
			}
			Assert.AreEqual(0, parser.errors.count, "There were parsing errors");
		}
	}
}