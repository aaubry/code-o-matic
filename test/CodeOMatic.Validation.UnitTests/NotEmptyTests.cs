using System;
using System.Collections.Generic;
using MbUnit.Framework;
using CodeOMatic.Validation;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class NotEmptyTests
	{
		private static void StringNotEmptyHelper([NotEmpty] string notEmpty)
		{
			if (notEmpty != null)
			{
				Assert.IsNotEmpty(notEmpty, "It should not be possible to invoke this method passing an empty string.");
			}
		}

		[Test]
		public void StringNotEmpty()
		{
			StringNotEmptyHelper("Hello");
		}

		[Test]
		public void StringNull()
		{
			StringNotEmptyHelper(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void StringEmpty()
		{
			StringNotEmptyHelper("");
		}

		private static void CollectionNotEmptyHelper([NotEmpty] int[] notEmpty)
		{
			if (notEmpty != null)
			{
				Assert.IsNotEmpty(notEmpty, "It should not be possible to invoke this method passing an empty collection.");
			}
		}

		[Test]
		public void CollectionNotEmpty()
		{
			CollectionNotEmptyHelper(new int[] { 1, 2 });
		}

		[Test]
		public void CollectionNull()
		{
			CollectionNotEmptyHelper(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CollectionEmpty()
		{
			CollectionNotEmptyHelper(new int[0]);
		}

		private static void CollectionOfTNotEmptyHelper([NotEmpty] IList<int> notEmpty)
		{
			if (notEmpty != null)
			{
				Assert.AreNotEqual(0, notEmpty.Count, "It should not be possible to invoke this method passing an empty collection.");
			}
		}

		[Test]
		public void CollectionOfTNotEmpty()
		{
			CollectionOfTNotEmptyHelper(new int[] { 1, 2 });
		}

		[Test]
		public void CollectionOfTNull()
		{
			CollectionOfTNotEmptyHelper(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CollectionOfTEmpty()
		{
			CollectionOfTNotEmptyHelper(new int[0]);
		}

		
		private class HelperClass
		{
			public void ValidatedMethod([NotEmpty] string text)
			{
				Assert.IsTrue(text.Length > 0);
			}

			public void NotValidatedMethod(string text)
			{
				
			}
		}

	}
}