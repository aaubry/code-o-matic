using System;
using MbUnit.Framework;
using CodeOMatic.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

		private class SelectorsForPropertiesAndFieldsWorkType
		{
			public string Name
			{
				get;
				set;
			}

			public string Email;
		}

		private void SelectorsForPropertiesAndFieldsWorkHelper(
			[NotNull(Selectors = ",Name,Email")]
			SelectorsForPropertiesAndFieldsWorkType value
		)
		{
			Assert.IsNotNull(value);
			Assert.IsNotNull(value.Name);
			Assert.IsNotNull(value.Email);
		}

		[Test]
		public void SelectorsForPropertiesAndFieldsWork()
		{
			SelectorsForPropertiesAndFieldsWorkHelper(new SelectorsForPropertiesAndFieldsWorkType { Name = "aaa", Email = "bbb" });
		}

		private void SelectorsForCollectionsWorkHelper(
			[NotNull(Selectors = "*.Values*")]
			IEnumerable<X> value
		)
		{
			Console.WriteLine("INSIDE METHOD");
		}

		[Test]
		public void SelectorsForCollectionsWork()
		{
			SelectorsForCollectionsWorkHelper(EnumerateSequence<X>(new[] { new X(EnumerateSequence<string>(new[] { "aaa", "bbb" })), new X(EnumerateSequence<string>(new[] { "ccc", "ddd" })) }));
		}

		private static void SelectorsHelper(
			[Values(true, Selectors = "CanRead")]
			Stream value
		)
		{
		}


		private class X
		{
			private IEnumerable<string> values;

			public X(IEnumerable<string> values)
			{
				this.values = values;
			}

			public IEnumerable<string> Values
			{
				get
				{
					Console.WriteLine("Reading the Values property");
					return values;
				}
			}
		}

		private IEnumerable<T> EnumerateSequence<T>(IEnumerable<T> sequence)
		{
			foreach (var item in sequence)
			{
				Console.WriteLine(item);
				yield return item;
			}
		}
	}
}