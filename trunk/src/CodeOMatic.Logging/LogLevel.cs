using System;

namespace CodeOMatic.Logging
{
	/// <summary>
	/// Indicates the severity of a log message.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Indicates no level.
		/// </summary>
		None,

		/// <summary>
		/// Indicates the DEBUG level.
		/// </summary>
		Debug,

		/// <summary>
		/// Indicates the INFO level.
		/// </summary>
		Info,

		/// <summary>
		/// Indicates the WARN level.
		/// </summary>
		Warn,

		/// <summary>
		/// Indicates the ERROR level.
		/// </summary>
		Error,

		/// <summary>
		/// Indicates the FATAL level.
		/// </summary>
		Fatal,
	}
}