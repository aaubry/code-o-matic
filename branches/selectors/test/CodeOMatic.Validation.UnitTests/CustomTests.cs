using System;
using MbUnit.Framework;
using System.Collections.Generic;
using CodeOMatic.Validation;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public abstract class CustomTests
	{
		[CustomValidator("ValidateArguments")]
		private void ValidateMethodHelper([CustomValidator("ValidateArg1")] string arg1, string arg2)
		{
			Assert.IsNotNull(arg1, "This method should never be called with null.");
			Assert.IsNotNull(arg2, "This method should never be called with null.");
		}

		private void ValidateArguments(IDictionary<string, object> parameters)
		{
			foreach(var pair in parameters)
			{
				if(pair.Value == null)
				{
					throw new ArgumentOutOfRangeException(pair.Key);
				}
			}
		}

		private void ValidateArg1(object parameterValue, string parameterName)
		{
			if("invalid".Equals(parameterValue))
			{
				throw new InvalidOperationException();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ValidateMethod()
		{
			ValidateMethodHelper(null, null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateParameter()
		{
			ValidateMethodHelper("invalid", "valid");
		}

		private void ValidateParameterWithSpecificMethodHelper1([CustomValidator("ValidateParameterWithSpecificMethodValidator1")] string argument)
		{
		}

		private void ValidateParameterWithSpecificMethodHelper2([CustomValidator("ValidateParameterWithSpecificMethodValidator2")] string argument)
		{
		}

		private void ValidateParameterWithSpecificMethodHelper3([CustomValidator("ValidateParameterWithSpecificMethodValidator3")] string argument)
		{
		}

		private void ValidateParameterWithSpecificMethodHelper4([CustomValidator("ValidateParameterWithSpecificMethodValidator4")] string argument)
		{
		}

		private static void ValidateParameterWithSpecificMethodHelper5([CustomValidator("ValidateParameterWithSpecificMethodValidator5")] string argument)
		{
		}

		private static void ValidateParameterWithSpecificMethodHelper6([CustomValidator("ValidateParameterWithSpecificMethodValidator6")] string argument)
		{
		}

		private void ValidateParameterWithSpecificMethodValidator1(string parameterValue, string parameterName)
		{
			throw new InvalidOperationException();
		}

		private void ValidateParameterWithSpecificMethodValidator2(string parameterValue)
		{
			throw new InvalidOperationException();
		}

		private static void ValidateParameterWithSpecificMethodValidator3(object target, string parameterValue, string parameterName)
		{
			throw new InvalidOperationException();
		}

		private static void ValidateParameterWithSpecificMethodValidator4(object target, string parameterValue)
		{
			throw new InvalidOperationException();
		}

		private static void ValidateParameterWithSpecificMethodValidator5(string parameterValue, string parameterName)
		{
			throw new InvalidOperationException();
		}

		private static void ValidateParameterWithSpecificMethodValidator6(string parameterValue)
		{
			throw new InvalidOperationException();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateParameterWithSpecificMethodWithName()
		{
			ValidateParameterWithSpecificMethodHelper1("invalid");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateParameterWithSpecificMethodWithoutName()
		{
			ValidateParameterWithSpecificMethodHelper2("invalid");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateParameterWithSpecificStaticMethodWithName()
		{
			ValidateParameterWithSpecificMethodHelper3("invalid");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValidateParameterWithSpecificStaticMethodWithoutName()
		{
			ValidateParameterWithSpecificMethodHelper4("invalid");
		}
	}
}