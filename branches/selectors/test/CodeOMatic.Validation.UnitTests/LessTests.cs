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
			StaticFirstLessThanSecondLessThanThird(3, 1, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void LessShouldFail2()
		{
			StaticFirstLessThanSecondLessThanThird(1, 3, 2);
		}

		[Test]
		public void GreaterShouldSucceed()
		{
			StaticFirstLessThanSecondLessThanThird(1, 2, 3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EqualShouldFail()
		{
			StaticFirstLessThanSecondLessThanThird(1, 1, 2);
		}

		[Less("first", "second")]
		[Less("second", "third")]
		private static void StaticFirstLessThanSecondLessThanThird(int first, int second, int third)
		{
			int result;
			Assert.IsTrue(first < second, "first must always be less that second.");
			Assert.IsTrue(second < third, "second must always be less that third.");
			result = first + second - third;
		}

		[Test]
		public void InstanceLessShouldSucceed()
		{
			InstanceFirstLessThanSecondLessThanThird(1, 2);
		}

		[Less("first", "second")]
		private void InstanceFirstLessThanSecondLessThanThird(object first, int second)
		{
			Assert.IsTrue((int)first < second, "first must always be less that second.");
		}
	}
}