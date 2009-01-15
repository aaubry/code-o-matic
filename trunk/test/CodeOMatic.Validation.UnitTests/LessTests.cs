using System;
using MbUnit.Framework;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class LessTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void LessShouldFail()
		{
			FirstLessThanSecondLessThanThird(3, 1, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void LessShouldFail2()
		{
			FirstLessThanSecondLessThanThird(1, 3, 2);
		}

		[Test]
		public void GreaterShouldSucceed()
		{
			FirstLessThanSecondLessThanThird(1, 2, 3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EqualShouldFail()
		{
			FirstLessThanSecondLessThanThird(1, 1, 2);
		}

		[Less("first", "second")]
		[Less("second", "third")]
		private static void FirstLessThanSecondLessThanThird(int first, int second, int third)
		{
			int result;
			Assert.IsTrue(first < second, "first must always be less that second.");
			Assert.IsTrue(second < third, "second must always be less that third.");
			result = first + second - third;
		}
	}
}