using System;
using MbUnit.Framework;
using CodeOMatic.Validation;

namespace CodeOMatic.Validation.UnitTests
{
    [TestFixture]
    public class PatternTests
    {
        /// <summary>
        /// Tests the email pattern helper.
        /// </summary>
        /// <param name="email">The email.</param>
        private static void TestEmailPatternHelper([NotNull][Pattern(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")] string email)
        {
            Assert.IsNotNull(email, "It should not be possible to invoke this method passing null.");
        }

        [Test]
        public void EmailPattern()
        {
            TestEmailPatternHelper("nuno.lourenco@fullsix.com");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmailPatternDefaultException()
        {
            TestEmailPatternHelper("nuno.lourenco%fullsix.com");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmailPatternNullContent()
        {
            TestEmailPatternHelper(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmailPatternEmptyContent()
        {
            TestEmailPatternHelper(string.Empty);
        }

		private static void TestNegation([Pattern(@"(.).*\1", Negate = true)] string text)
		{
		}

		[Test]
		public void NegationShouldPass()
		{
			TestNegation("ABCDEF");
		}

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NegationShouldFail()
        {
			TestNegation("ABCDBEF");
        }
    }
}