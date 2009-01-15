using System;
using MbUnit.Framework;
using CodeOMatic.Validation;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class NotNullTests
	{
		private static void TestNotNullHelper([NotNull] string notNull)
		{
			Assert.IsNotNull(notNull, "It should not be possible to invoke this method passing null.");
		}

		[Test]
		public void NotNull()
		{
			TestNotNullHelper("Hello");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Null()
		{
			TestNotNullHelper(null);
		}

		private static void TestNotNullHelperWithMessage([NotNull(Message = "I am null")] string notNull)
		{
			Assert.IsNotNull(notNull, "It should not be possible to invoke this method passing null.");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), Description = "I am null")]
		public void NullWithMessage()
		{
			TestNotNullHelperWithMessage(null);
		}

		private static void TestNotNullHelperWithException([NotNull(Exception = typeof(ArgumentException))] string notNull)
		{
			Assert.IsNotNull(notNull, "It should not be possible to invoke this method passing null.");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NullWithException()
		{
			TestNotNullHelperWithMessage(null);
		}

		private static void TestNotNullHelperWithExceptionAndMessage([NotNull(Exception = typeof(ArgumentException), Message = "I am null")] string notNull)
		{
			Assert.IsNotNull(notNull, "It should not be possible to invoke this method passing null.");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), Description = "I am null")]
		public void NullWithExceptionAndMessage()
		{
			TestNotNullHelperWithExceptionAndMessage(null);
		}

		private static void ThrowExceptionHelper()
		{
			throw new NullReferenceException("ThrowExceptionHelper");
		}

		[Test]
		[ExpectedException(typeof(NullReferenceException), Description = "ThrowExceptionHelper")]
		public void ThrowExceptionFromMethod()
		{
			ThrowExceptionHelper();
		}

		[NotNull]
		private string HelperProperty
		{
			get
			{
				return null;
			}
			set
			{
				Assert.IsNotNull(value, "It should not be possible to set this property to null.");
			}
		}

		[NotNull]
		private static string StaticHelperProperty
		{
			get
			{
				return null;
			}
			set
			{
				Assert.IsNotNull(value, "It should not be possible to set this property to null.");
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PropertyNotNull()
		{
			HelperProperty = null;
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void StaticPropertyNotNull()
		{
			StaticHelperProperty = null;
		}
	}
}