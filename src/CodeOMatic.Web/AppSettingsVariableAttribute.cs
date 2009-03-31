using System;
using System.Configuration;
using System.Globalization;

namespace CodeOMatic.Web
{
	/// <summary>
	/// Implements an abstract property as an AppSettings variable.
	/// </summary>
	[Serializable]
	public sealed class AppSettingsVariableAttribute : CollectionVariableAttribute
	{
		private Type type;

		/// <summary>
		/// Allows derived classes to perform compile-time initializations.
		/// </summary>
		/// <param name="propertyType">Type of the property.</param>
		protected override void CompileTimeInitialize(Type propertyType)
		{
			type = propertyType;
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <returns></returns>
		protected override object GetValue(object target)
		{
			string value = ConfigurationManager.AppSettings[Key];
			if (value != null)
			{
				return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
			} else
			{
				return GetDefaultValue(target);
			}
		}

		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="target">The object on which the property should be set.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		protected override void SetValue(object target, object value, object defaultValue)
		{
			throw new NotSupportedException("Writing to an AppSettings property is not supported.");
		}
	}
}