using System;
using System.Web.UI;

namespace CodeOMatic.Web.UnitTests
{
	public abstract class SomeControl : Control
	{
		[SessionVariable]
		protected abstract string MyVariable
		{
			get;
			set;
		}
	}
}