using System;

namespace CodeOMatic.Validation.Core
{
	/// <summary>
	/// 
	/// </summary>
	public interface IValidator
	{
		/// <summary>
		/// Gets a value indicating whether this instance is persistent.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is persistent; otherwise, <c>false</c>.
		/// </value>
		bool IsPersistent
		{
			get;
		}
	}
}