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

		private static void DateIntervalHelper([Interval("[date] 2008-10-24", "[date] 2009-4-16")] DateTime value)
		{
			
		}

		[Test]
		public void ValidateDates_Valid()
		{
			DateIntervalHelper(new DateTime(2008, 11, 1));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ValidateDates_Less()
		{
			DateIntervalHelper(new DateTime(2007, 11, 1));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ValidateDates_Greater()
		{
			DateIntervalHelper(new DateTime(2009, 11, 1));
		}
	}
}
