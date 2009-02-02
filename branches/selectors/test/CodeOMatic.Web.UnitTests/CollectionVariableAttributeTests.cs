using System;
using MbUnit.Framework;

namespace CodeOMatic.Web.UnitTests
{
	[TestFixture]
	public abstract class CollectionVariableAttributeTests
	{
		[TestingCollectionVariableAttribute]
		protected abstract string NoDefaultValue
		{
			get;
			set;
		}

		[TestingCollectionVariableAttribute(DefaultValue = "ContantDefaultValue")]
		protected abstract string ContantDefaultValue
		{
			get;
			set;
		}

		[TestingCollectionVariableAttribute(DefaultValueMethod = "InstanceGetMethodDefaultValue")]
		protected abstract string InstanceMethodDefaultValue
		{
			get;
			set;
		}

		[TestingCollectionVariableAttribute(DefaultValueMethod = "StaticGetMethodDefaultValue")]
		protected abstract string StaticMethodDefaultValue
		{
			get;
			set;
		}

		private string InstanceGetMethodDefaultValue()
		{
			return "InstanceGetMethodDefaultValue";
		}

		private static string StaticGetMethodDefaultValue()
		{
			return "StaticGetMethodDefaultValue";
		}

		[Test]
		public void GetDefaultValueNull()
		{
			Assert.IsNull(NoDefaultValue, "When no default value is specified, the default should be null.");
		}

		[Test]
		public void GetDefaultValueConstant()
		{
			Assert.AreEqual("ContantDefaultValue", ContantDefaultValue, "When a constant default value is specified, the default should be that value.");
		}

		[Test]
		public void GetDefaultValueInstanceMethod()
		{
			Assert.AreEqual(InstanceGetMethodDefaultValue(), InstanceMethodDefaultValue, "When a method default value is specified, the default should be obtained by calling that method.");
		}

		[Test]
		public void GetDefaultValueStaticMethod()
		{
			Assert.AreEqual(StaticGetMethodDefaultValue(), StaticMethodDefaultValue, "When a method default value is specified, the default should be obtained by calling that method.");
		}

		[TestingCollectionVariableAttribute(DefaultValueMethod = "GetDefaultDate")]
		protected abstract DateTime DefaultDate
		{
			get;
			set;
		}

		private DateTime GetDefaultDate()
		{
			return DateTime.Today;
		}
	}

	[Serializable]
	public sealed class TestingCollectionVariableAttribute : CollectionVariableAttribute
	{
		protected override object GetValue(object target)
		{
			return GetDefaultValue(target);
		}

		protected override void SetValue(object target, object value, object defaultValue)
		{
			throw new NotImplementedException();
		}
	}
}