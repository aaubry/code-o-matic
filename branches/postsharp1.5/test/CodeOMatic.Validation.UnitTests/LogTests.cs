using System;
using MbUnit.Framework;
using CodeOMatic.Logging;
using log4net;

namespace CodeOMatic.Validation.UnitTests
{
	[TestFixture]
	public class LogTests
	{
		private class A<T>
		{
			public T Value
			{
				get;
				set;
			}

			public A()
			{
				Log.Info(Value);
			}
		}

		private class B<T>
		{
			private static readonly ILog log;

			public T Value
			{
				get;
				set;
			}

			static B()
			{
				log = LogManager.GetLogger(typeof(B<T>));
			}
		}

		[Test]
		public void GenericsTest()
		{
			var a = new A<int>();
			Console.WriteLine(a.Value);
		}

		[Test]
		public void AttributeTest()
		{
			LoggedMethodStatic("Hello", "world");
			LoggedMethodInstance("Hello", "world");
		}

		[Log]
		private static void LoggedMethodStatic(string p, string p_2)
		{
		}

		[Log]
		private void LoggedMethodInstance(string p, string p_2)
		{
		}
	}
}