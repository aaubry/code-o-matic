using System;
using System.Web.UI;
using CodeOMatic.Validation;
using MbUnit.Framework;

namespace CodeOMatic.Web.UnitTests
{
	public abstract class SomeControl : Control
	{
		[SessionVariable]
		public abstract string MySessionVariable
		{
			get;
			set;
		}

		[ViewStateVariable]
		public abstract string MyViewStateVariable
		{
			get;
			set;
		}

		[NotNull]
		[ViewStateVariable]
		public extern string MyViewStateVariable2
		{
			get;
			set;
		}
	}

	[TestFixture]
	public class SomeControlTests
	{
		private SomeControl Control
		{
			get
			{
				return Activator.CreateInstance<SomeControl>();
			}
		}

		[Test]
		public void TestControl()
		{
			Control.MyViewStateVariable = "Hello";
			Console.WriteLine(Control.MyViewStateVariable);
		}
	}
}