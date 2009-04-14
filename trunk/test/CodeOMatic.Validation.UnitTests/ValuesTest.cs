using System;
using MbUnit.Framework;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class ValuesTest
	{
		private static void ValuesHelper(
			[Values("good", "nice")]
			string value
		)
		{
		}

		[Row("good")]
		[Row("bad", ExpectedException = typeof(ArgumentException))]
		[Row("ugly", ExpectedException = typeof(ArgumentException))]
		[RowTest]
		public void Values(string value)
		{
			ValuesHelper(value);
		}
	}
}