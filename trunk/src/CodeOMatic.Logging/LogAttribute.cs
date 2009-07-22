using System;

namespace CodeOMatic.Logging
{
	/// <summary>
	/// Makes the method write log messages when entering and leaving the method,
	/// as well as when the method throws an exception.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class LogAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the message that is displayed when the method is invoked.
		/// </summary>
		public string EntryMessage
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the message that is displayed when the method returns.
		/// </summary>
		public string ExitMessage
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the message that is displayed when the method throws an exception.
		/// </summary>
		public string ExceptionMessage
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the severity of the message that is displayed when the method is invoked.
		/// </summary>
		public LogLevel EntryLevel
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the severity of the message that is displayed when the method returns.
		/// </summary>
		public LogLevel ExitLevel
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the severity of the message that is displayed when the method throws an exception.
		/// </summary>
		public LogLevel ExceptionLevel
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogAttribute"/> class.
		/// </summary>
		public LogAttribute()
		{
			EntryLevel = LogLevel.Debug;
			ExitLevel = LogLevel.Debug;
			ExceptionLevel = LogLevel.Error;
		}
	}
}