using System;
using MbUnit.Framework;
using CodeOMatic.Validation;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class IntervalTests
	{
		private void IntervalHelper([Interval(10, 20, MinMode = BoundaryMode.Exclusive, MaxMode = BoundaryMode.Inclusive)] int value)
		{
			Assert.IsTrue(value > 10, "The value must be greater than 10.");
			Assert.IsTrue(value <= 20, "The value must not be greater than 20.");
		}

		[Test]
		public void ValueInRange()
		{
			IntervalHelper(12);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ValueEqualsMinimum()
		{
			IntervalHelper(10);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ValueLessThanMinimum()
		{
			IntervalHelper(5);
		}

		[Test]
		public void ValueEqualToMaximum()
		{
			IntervalHelper(20);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ValueGreaterThanMaximum()
		{
			IntervalHelper(24);
		}
	}
}
