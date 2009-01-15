using System;
using System.Globalization;

namespace CodeOMatic.Validation.CompileTime
{
	/// <summary>
	/// Generates unique names for variables and fields.
	/// </summary>
	public static class NameGenerator
	{
		/// <summary>
		/// Generates a new unique name.
		/// </summary>
		/// <param name="baseName">A string that will be included in the generated name to make it easier to know what it is about.</param>
		/// <returns></returns>
		public static string Generate(string baseName)
		{
			return string.Format(CultureInfo.InvariantCulture, "__~~~{0}_{1}", baseName, Guid.NewGuid());
		}
	}
}