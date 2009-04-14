using System;
using CodeOMatic.Validation.Core;
using MbUnit.Framework;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class StringConverterTests
	{
		private static void PerformTest(string text, object expected)
		{
			Assert.AreEqual(expected, StringConverter.Convert(text), text);
		}

		[Test]
		public void UnformattedString()
		{
			PerformTest("hello", "hello");
		}

		[Test]
		public void Int32()
		{
			PerformTest("[Int32] 20", 20);
		}

		[Test]
		public void Alias()
		{
			PerformTest("[float] 20.25", 20.25F);
		}

		[Test]
		public void SystemInt32()
		{
			PerformTest("[System.Int32] 32", 32);
		}

		[Test]
		public void SystemInt32Mscorlib()
		{
			PerformTest("[System.Int32, mscorlib] 61", 61);
		}

		[Test]
		public void DefaultString()
		{
			PerformTest("[] hello", "hello");
		}
		
		[Test]
		public void String()
		{
			PerformTest("[String] hello", "hello");
		}

		[Test]
		public void CustomParse()
		{
			PerformTest("[CodeOMatic.Validation.UnitTests.StringConverterTests_A] 13", 26);
		}

		[Test]
		public void CustomParseWithFullName()
		{
			PerformTest("[CodeOMatic.Validation.UnitTests.StringConverterTests_A, CodeOMatic.Validation.UnitTests] 34", 68);
		}

		[Test]
		public void Date()
		{
			PerformTest("[date] 1980-07-18", new DateTime(1980, 7, 18));
		}

		[Test]
		public void DateTime()
		{
			PerformTest("[date] 1980-07-18 10:15:46", new DateTime(1980, 7, 18, 10, 15, 46));
		}

		[Row("[ invalid", ExpectedException = typeof(ArgumentException))]
		[Row("[int]invalid", ExpectedException = typeof(ArgumentException))]
		[RowTest]
		public void SanityChecks(string value)
		{
			StringConverter.Convert(value);
		}
	}

	public class StringConverterTests_A
	{
		public static int Parse(string value)
		{
			return int.Parse(value) * 2;
		}
	}
}