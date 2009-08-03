using System;
using System.ComponentModel;
using log4net;
using System.Text;
using System.Globalization;

namespace CodeOMatic.Logging
{
	/// <summary>
	/// The Log class is used by application to log messages into
	/// the log4net framework.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class contains methods for logging at different levels and also
	/// has properties for determining if those logging levels are
	/// enabled in the current configuration.
	/// </para>
	/// </remarks>
	/// <example>Simple example of logging messages
	/// <code lang="C#">
	/// Log.Info("Application Start");
	/// Log.Debug("This is a debug message");
	/// 
	/// if (Log.IsDebugEnabled)
	/// {
	/// 	Log.Debug("This is another debug message");
	/// }
	/// </code>
	/// </example>
	public static class Log
	{
		#region Logger
		/// <summary>
		/// Wrapper for the <see cref="ILog"/> interface. Do not use this class.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This class is an implementation detail and should not be user in code. Use the static methods of the Log class instead.", true)]
		public static class Logger
		{
			#region Logging methods
			/// <summary>
			/// Log a message object with the <see cref="F:log4net.Core.Level.Debug"/> level including
			/// the stack trace of the <see cref="T:System.Exception"/> passed
			/// as a parameter.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="exception">The exception to log, including its stack trace.</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// See the <see cref="M:log4net.ILog.Debug(System.Object)"/> form for more detailed information.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static void Debug(object message, Exception exception, ILog log)
			{
				log.Debug(message, exception);
			}

			/// <summary>
			/// Log a message object with the <see cref="F:log4net.Core.Level.Debug"/> level.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Debug"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// This method first checks if this logger is <c>DEBUG</c>
			/// enabled by comparing the level of this logger with the
			/// <see cref="F:log4net.Core.Level.Debug"/> level. If this logger is
			/// <c>DEBUG</c> enabled, then it converts the message object
			/// (passed as parameter) to a string by invoking the appropriate
			/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
			/// proceeds to call all the registered appenders in this logger
			/// and also higher in the hierarchy depending on the value of
			/// the additivity flag.
			/// </para>
			/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
			/// to this method will print the name of the <see cref="T:System.Exception"/>
			/// but no stack trace. To print a stack trace use the
			/// <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/> form instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static void Debug(object message, ILog log)
			{
				log.Debug(message);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
			/// </summary>
			/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static void DebugFormat(IFormatProvider provider, string format, object[] args, ILog log)
			{
				log.DebugFormat(provider, format, args);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="arg2">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static void DebugFormat(string format, object arg0, object arg1, object arg2, ILog log)
			{
				log.DebugFormat(format, arg0, arg1, arg2);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static void DebugFormat(string format, object arg0, object arg1, ILog log)
			{
				log.DebugFormat(format, arg0, arg1);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static void DebugFormat(string format, object arg0, ILog log)
			{
				log.DebugFormat(format, arg0);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a formatted string with the <see cref="F:log4net.Core.Level.Debug"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static void DebugFormat(string format, object[] args, ILog log)
			{
				log.DebugFormat(CultureInfo.InvariantCulture, format, args);
			}

			/// <summary>
			/// Log a message object with the <see cref="F:log4net.Core.Level.Error"/> level including
			/// the stack trace of the <see cref="T:System.Exception"/> passed
			/// as a parameter.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="exception">The exception to log, including its stack trace.</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// See the <see cref="M:log4net.ILog.Error(System.Object)"/> form for more detailed information.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
			public static void Error(object message, Exception exception, ILog log)
			{
				log.Error(message, exception);
			}

			/// <summary>
			/// Logs a message object with the <see cref="F:log4net.Core.Level.Error"/> level.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Error"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// This method first checks if this logger is <c>ERROR</c>
			/// enabled by comparing the level of this logger with the
			/// <see cref="F:log4net.Core.Level.Error"/> level. If this logger is
			/// <c>ERROR</c> enabled, then it converts the message object
			/// (passed as parameter) to a string by invoking the appropriate
			/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
			/// proceeds to call all the registered appenders in this logger
			/// and also higher in the hierarchy depending on the value of the
			/// additivity flag.
			/// </para>
			/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
			/// to this method will print the name of the <see cref="T:System.Exception"/>
			/// but no stack trace. To print a stack trace use the
			/// <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/> form instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
			public static void Error(object message, ILog log)
			{
				log.Error(message);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
			/// </summary>
			/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
			public static void ErrorFormat(IFormatProvider provider, string format, object[] args, ILog log)
			{
				log.ErrorFormat(provider, format, args);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="arg2">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
			public static void ErrorFormat(string format, object arg0, object arg1, object arg2, ILog log)
			{
				log.ErrorFormat(format, arg0, arg1, arg2);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
			public static void ErrorFormat(string format, object arg0, object arg1, ILog log)
			{
				log.ErrorFormat(format, arg0, arg1);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
			public static void ErrorFormat(string format, object arg0, ILog log)
			{
				log.ErrorFormat(format, arg0);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
			public static void ErrorFormat(string format, object[] args, ILog log)
			{
				log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
			}

			/// <summary>
			/// Log a message object with the <see cref="F:log4net.Core.Level.Fatal"/> level including
			/// the stack trace of the <see cref="T:System.Exception"/> passed
			/// as a parameter.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="exception">The exception to log, including its stack trace.</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// See the <see cref="M:log4net.ILog.Fatal(System.Object)"/> form for more detailed information.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
			public static void Fatal(object message, Exception exception, ILog log)
			{
				log.Fatal(message, exception);
			}

			/// <summary>
			/// Log a message object with the <see cref="F:log4net.Core.Level.Fatal"/> level.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Fatal"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// This method first checks if this logger is <c>FATAL</c>
			/// enabled by comparing the level of this logger with the
			/// <see cref="F:log4net.Core.Level.Fatal"/> level. If this logger is
			/// <c>FATAL</c> enabled, then it converts the message object
			/// (passed as parameter) to a string by invoking the appropriate
			/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
			/// proceeds to call all the registered appenders in this logger
			/// and also higher in the hierarchy depending on the value of the
			/// additivity flag.
			/// </para>
			/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
			/// to this method will print the name of the <see cref="T:System.Exception"/>
			/// but no stack trace. To print a stack trace use the
			/// <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/> form instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
			public static void Fatal(object message, ILog log)
			{
				log.Fatal(message);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
			/// </summary>
			/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
			public static void FatalFormat(IFormatProvider provider, string format, object[] args, ILog log)
			{
				log.FatalFormat(provider, format, args);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="arg2">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
			public static void FatalFormat(string format, object arg0, object arg1, object arg2, ILog log)
			{
				log.FatalFormat(format, arg0, arg1, arg2);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
			public static void FatalFormat(string format, object arg0, object arg1, ILog log)
			{
				log.FatalFormat(format, arg0, arg1);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
			public static void FatalFormat(string format, object arg0, ILog log)
			{
				log.FatalFormat(format, arg0);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
			public static void FatalFormat(string format, object[] args, ILog log)
			{
				log.FatalFormat(CultureInfo.InvariantCulture, format, args);
			}

			/// <summary>
			/// Logs a message object with the <c>INFO</c> level including
			/// the stack trace of the <see cref="T:System.Exception"/> passed
			/// as a parameter.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="exception">The exception to log, including its stack trace.</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// See the <see cref="M:log4net.ILog.Info(System.Object)"/> form for more detailed information.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
			public static void Info(object message, Exception exception, ILog log)
			{
				log.Info(message, exception);
			}

			/// <summary>
			/// Logs a message object with the <see cref="F:log4net.Core.Level.Info"/> level.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Info"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// This method first checks if this logger is <c>INFO</c>
			/// enabled by comparing the level of this logger with the
			/// <see cref="F:log4net.Core.Level.Info"/> level. If this logger is
			/// <c>INFO</c> enabled, then it converts the message object
			/// (passed as parameter) to a string by invoking the appropriate
			/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
			/// proceeds to call all the registered appenders in this logger
			/// and also higher in the hierarchy depending on the value of the
			/// additivity flag.
			/// </para>
			/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
			/// to this method will print the name of the <see cref="T:System.Exception"/>
			/// but no stack trace. To print a stack trace use the
			/// <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/> form instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
			public static void Info(object message, ILog log)
			{
				log.Info(message);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
			/// </summary>
			/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
			public static void InfoFormat(IFormatProvider provider, string format, object[] args, ILog log)
			{
				log.InfoFormat(provider, format, args);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="arg2">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
			public static void InfoFormat(string format, object arg0, object arg1, object arg2, ILog log)
			{
				log.InfoFormat(format, arg0, arg1, arg2);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
			public static void InfoFormat(string format, object arg0, object arg1, ILog log)
			{
				log.InfoFormat(format, arg0, arg1);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
			public static void InfoFormat(string format, object arg0, ILog log)
			{
				log.InfoFormat(format, arg0);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
			public static void InfoFormat(string format, object[] args, ILog log)
			{
				log.InfoFormat(CultureInfo.InvariantCulture, format, args);
			}

			/// <summary>
			/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Debug"/> level.
			/// </summary>
			/// <param name="log">The log.</param>
			/// <returns></returns>
			/// <value>
			/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Debug"/> events, <c>false</c> otherwise.
			/// </value>
			/// <remarks>
			/// 	<para>
			/// This function is intended to lessen the computational cost of
			/// disabled log debug statements.
			/// </para>
			/// 	<para> For some ILog interface <c>log</c>, when you write:</para>
			/// 	<code lang="C#">
			/// log.Debug("This is entry number: " + i );
			/// </code>
			/// 	<para>
			/// You incur the cost constructing the message, string construction and concatenation in
			/// this case, regardless of whether the message is logged or not.
			/// </para>
			/// 	<para>
			/// If you are worried about speed (who isn't), then you should write:
			/// </para>
			/// 	<code lang="C#">
			/// if (log.IsDebugEnabled)
			/// {
			/// log.Debug("This is entry number: " + i );
			/// }
			/// </code>
			/// 	<para>
			/// This way you will not incur the cost of parameter
			/// construction if debugging is disabled for <c>log</c>. On
			/// the other hand, if the <c>log</c> is debug enabled, you
			/// will incur the cost of evaluating whether the logger is debug
			/// enabled twice. Once in <see cref="P:log4net.ILog.IsDebugEnabled"/> and once in
			/// the <see cref="M:log4net.ILog.Debug(System.Object)"/>.  This is an insignificant overhead
			/// since evaluating a logger takes about 1% of the time it
			/// takes to actually log. This is the preferred style of logging.
			/// </para>
			/// 	<para>Alternatively if your logger is available statically then the is debug
			/// enabled state can be stored in a static variable like this:
			/// </para>
			/// 	<code lang="C#">
			/// private static readonly bool isDebugEnabled = log.IsDebugEnabled;
			/// </code>
			/// 	<para>
			/// Then when you come to log you can write:
			/// </para>
			/// 	<code lang="C#">
			/// if (isDebugEnabled)
			/// {
			/// log.Debug("This is entry number: " + i );
			/// }
			/// </code>
			/// 	<para>
			/// This way the debug enabled state is only queried once
			/// when the class is loaded. Using a <c>private static readonly</c>
			/// variable is the most efficient because it is a run time constant
			/// and can be heavily optimized by the JIT compiler.
			/// </para>
			/// 	<para>
			/// Of course if you use a static readonly variable to
			/// hold the enabled state of the logger then you cannot
			/// change the enabled state at runtime to vary the logging
			/// that is produced. You have to decide if you need absolute
			/// speed or runtime flexibility.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
			/// <seealso cref="M:log4net.ILog.DebugFormat(System.IFormatProvider,System.String,System.Object[])"/>
			public static bool get_IsDebugEnabled(ILog log)
			{
				return log.IsDebugEnabled;
			}

			/// <summary>
			/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Error"/> level.
			/// </summary>
			/// <param name="log">The log.</param>
			/// <returns></returns>
			/// <value>
			/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Error"/> events, <c>false</c> otherwise.
			/// </value>
			/// <remarks>
			/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
			/// <seealso cref="M:log4net.ILog.ErrorFormat(System.IFormatProvider,System.String,System.Object[])"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static bool get_IsErrorEnabled(ILog log)
			{
				return log.IsErrorEnabled;
			}

			/// <summary>
			/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Fatal"/> level.
			/// </summary>
			/// <param name="log">The log.</param>
			/// <returns></returns>
			/// <value>
			/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Fatal"/> events, <c>false</c> otherwise.
			/// </value>
			/// <remarks>
			/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
			/// <seealso cref="M:log4net.ILog.FatalFormat(System.IFormatProvider,System.String,System.Object[])"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static bool get_IsFatalEnabled(ILog log)
			{
				return log.IsFatalEnabled;
			}

			/// <summary>
			/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Info"/> level.
			/// </summary>
			/// <param name="log">The log.</param>
			/// <returns>
			/// 	<c>true</c> if [is info enabled] [the specified log]; otherwise, <c>false</c>.
			/// </returns>
			/// <value>
			/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Info"/> events, <c>false</c> otherwise.
			/// </value>
			/// <remarks>
			/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
			/// <seealso cref="M:log4net.ILog.InfoFormat(System.IFormatProvider,System.String,System.Object[])"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static bool IsInfoEnabled(ILog log)
			{
				return log.IsInfoEnabled;
			}

			/// <summary>
			/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Warn"/> level.
			/// </summary>
			/// <param name="log">The log.</param>
			/// <returns></returns>
			/// <value>
			/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Warn"/> events, <c>false</c> otherwise.
			/// </value>
			/// <remarks>
			/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
			/// <seealso cref="M:log4net.ILog.WarnFormat(System.IFormatProvider,System.String,System.Object[])"/>
			/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
			public static bool get_IsWarnEnabled(ILog log)
			{
				return log.IsWarnEnabled;
			}

			/// <summary>
			/// Log a message object with the <see cref="F:log4net.Core.Level.Warn"/> level including
			/// the stack trace of the <see cref="T:System.Exception"/> passed
			/// as a parameter.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="exception">The exception to log, including its stack trace.</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// See the <see cref="M:log4net.ILog.Warn(System.Object)"/> form for more detailed information.
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
			public static void Warn(object message, Exception exception, ILog log)
			{
				log.Warn(message, exception);
			}

			/// <summary>
			/// Log a message object with the <see cref="F:log4net.Core.Level.Warn"/> level.
			/// </summary>
			/// <param name="message">The message object to log.</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Warn"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// This method first checks if this logger is <c>WARN</c>
			/// enabled by comparing the level of this logger with the
			/// <see cref="F:log4net.Core.Level.Warn"/> level. If this logger is
			/// <c>WARN</c> enabled, then it converts the message object
			/// (passed as parameter) to a string by invoking the appropriate
			/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
			/// proceeds to call all the registered appenders in this logger
			/// and also higher in the hierarchy depending on the value of the
			/// additivity flag.
			/// </para>
			/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
			/// to this method will print the name of the <see cref="T:System.Exception"/>
			/// but no stack trace. To print a stack trace use the
			/// <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/> form instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
			public static void Warn(object message, ILog log)
			{
				log.Warn(message);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
			/// </summary>
			/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
			public static void WarnFormat(IFormatProvider provider, string format, object[] args, ILog log)
			{
				log.WarnFormat(provider, format, args);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="arg2">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
			public static void WarnFormat(string format, object arg0, object arg1, object arg2, ILog log)
			{
				log.WarnFormat(format, arg0, arg1, arg2);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="arg1">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
			public static void WarnFormat(string format, object arg0, object arg1, ILog log)
			{
				log.WarnFormat(format, arg0, arg1);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="arg0">An Object to format</param>
			/// <param name="log">The log.</param>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
			/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
			public static void WarnFormat(string format, object arg0, ILog log)
			{
				log.WarnFormat(format, arg0);
			}

			/// <summary>
			/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
			/// </summary>
			/// <param name="format">A String containing zero or more format items</param>
			/// <param name="args">An Object array containing zero or more objects to format</param>
			/// <param name="log">The log.</param>
			/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.</overloads>
			/// <remarks>
			/// 	<para>
			/// The message is formatted using the <c>String.Format</c> method. See
			/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
			/// of the formatting.
			/// </para>
			/// 	<para>
			/// This method does not take an <see cref="T:System.Exception"/> object to include in the
			/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object)"/>
			/// methods instead.
			/// </para>
			/// </remarks>
			/// <seealso cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
			/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
			public static void WarnFormat(string format, object[] args, ILog log)
			{
				log.WarnFormat(CultureInfo.InvariantCulture, format, args);
			}
			#endregion
		}

		/// <summary>
		/// Do not use this class.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		//[Obsolete("This class is an implementation detail and should not be user in code. Use the static methods of the Log class instead.", true)]
		public static class LogAttributeHelper
		{
			private static string MakeEnterMessage(string methodName, string message, string[] argumentNames, object[] arguments)
			{
				StringBuilder entry = new StringBuilder();
				entry.AppendFormat(CultureInfo.InvariantCulture, "Entering method {0}: {1}", methodName, message);
				for(int i = 0; i < argumentNames.Length; ++i)
				{
					entry.AppendFormat(CultureInfo.InvariantCulture, "\n  {0} = {1}", argumentNames[i], Convert.ToString(arguments[i], CultureInfo.InvariantCulture));
				}
				return entry.ToString();
			}

			/// <summary/>
			public static void EnterDebug(string methodName, string message, string[] argumentNames, object[] arguments, ILog log)
			{
				log.Debug(MakeEnterMessage(methodName, message, argumentNames, arguments));
			}

			/// <summary/>
			public static void EnterInfo(string methodName, string message, string[] argumentNames, object[] arguments, ILog log)
			{
				log.Info(MakeEnterMessage(methodName, message, argumentNames, arguments));
			}

			/// <summary/>
			public static void EnterWarn(string methodName, string message, string[] argumentNames, object[] arguments, ILog log)
			{
				log.Warn(MakeEnterMessage(methodName, message, argumentNames, arguments));
			}

			/// <summary/>
			public static void EnterError(string methodName, string message, string[] argumentNames, object[] arguments, ILog log)
			{
				log.Error(MakeEnterMessage(methodName, message, argumentNames, arguments));
			}

			/// <summary/>
			public static void EnterFatal(string methodName, string message, string[] argumentNames, object[] arguments, ILog log)
			{
				log.Fatal(MakeEnterMessage(methodName, message, argumentNames, arguments));
			}

			private static string MakeLeaveMessage(string methodName, string message, bool hasReturnValue, object returnValue)
			{
				StringBuilder entry = new StringBuilder();
				entry.AppendFormat(CultureInfo.InvariantCulture, "Leaving method {0}: {1}", methodName, message);
				if (hasReturnValue)
				{
					entry.AppendFormat(CultureInfo.InvariantCulture, "\n  Return value = {0}", Convert.ToString(returnValue, CultureInfo.InvariantCulture));
				}
				return entry.ToString();
			}

			/// <summary/>
			public static void LeaveDebug(string methodName, string message, bool hasReturnValue, object returnValue, ILog log)
			{
				log.Debug(MakeLeaveMessage(methodName, message, hasReturnValue, returnValue));
			}

			/// <summary/>
			public static void LeaveInfo(string methodName, string message, bool hasReturnValue, object returnValue, ILog log)
			{
				log.Info(MakeLeaveMessage(methodName, message, hasReturnValue, returnValue));
			}

			/// <summary/>
			public static void LeaveWarn(string methodName, string message, bool hasReturnValue, object returnValue, ILog log)
			{
				log.Warn(MakeLeaveMessage(methodName, message, hasReturnValue, returnValue));
			}

			/// <summary/>
			public static void LeaveError(string methodName, string message, bool hasReturnValue, object returnValue, ILog log)
			{
				log.Error(MakeLeaveMessage(methodName, message, hasReturnValue, returnValue));
			}

			/// <summary/>
			public static void LeaveFatal(string methodName, string message, bool hasReturnValue, object returnValue, ILog log)
			{
				log.Fatal(MakeLeaveMessage(methodName, message, hasReturnValue, returnValue));
			}

			private static string MakeExceptionMessage(string methodName, string message)
			{
				return string.Format(CultureInfo.InvariantCulture, "Exception in method {0}: {1}", methodName, message);
			}

			/// <summary/>
			public static void ExceptionDebug(Exception exception, string methodName, string message, ILog log)
			{
				log.Debug(MakeExceptionMessage(methodName, message), exception);
			}

			/// <summary/>
			public static void ExceptionInfo(Exception exception, string methodName, string message, ILog log)
			{
				log.Info(MakeExceptionMessage(methodName, message), exception);
			}

			/// <summary/>
			public static void ExceptionWarn(Exception exception, string methodName, string message, ILog log)
			{
				log.Warn(MakeExceptionMessage(methodName, message), exception);
			}

			/// <summary/>
			public static void ExceptionError(Exception exception, string methodName, string message, ILog log)
			{
				log.Error(MakeExceptionMessage(methodName, message), exception);
			}

			/// <summary/>
			public static void ExceptionFatal(Exception exception, string methodName, string message, ILog log)
			{
				log.Fatal(MakeExceptionMessage(methodName, message), exception);
			}
		}
		#endregion

		#region Internal stuff
		private static readonly Type loggerType = typeof(Log).GetNestedType("Logger");

		/// <summary>
		/// Gets the type of the logger wrapper.
		/// </summary>
		/// <value>The type of the logger.</value>
		internal static Type LoggerType
		{
			get
			{
				return loggerType;
			}
		}

		private static Exception GetPublicMethodInvokedException()
		{
			return new NotSupportedException("This method call should have been replaced by a call to the Logger class. Please make sure that you are using PostSharp and that you do not call this method using reflection.");
		}
		#endregion

		#region Public interface
		/// <summary>
		/// Log a message object with the <see cref="F:log4net.Core.Level.Debug"/> level including
		/// the stack trace of the <see cref="T:System.Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// See the <see cref="M:log4net.ILog.Debug(System.Object)"/> form for more detailed information.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static void Debug(object message, Exception exception)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Log a message object with the <see cref="F:log4net.Core.Level.Debug"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Debug"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// This method first checks if this logger is <c>DEBUG</c>
		/// enabled by comparing the level of this logger with the
		/// <see cref="F:log4net.Core.Level.Debug"/> level. If this logger is
		/// <c>DEBUG</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
		/// proceeds to call all the registered appenders in this logger
		/// and also higher in the hierarchy depending on the value of
		/// the additivity flag.
		/// </para>
		/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
		/// to this method will print the name of the <see cref="T:System.Exception"/>
		/// but no stack trace. To print a stack trace use the
		/// <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/> form instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static void Debug(object message)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
		/// </summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <param name="arg2">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static void DebugFormat(string format, object arg0, object arg1, object arg2)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static void DebugFormat(string format, object arg0, object arg1)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static void DebugFormat(string format, object arg0)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <overloads>Log a formatted string with the <see cref="F:log4net.Core.Level.Debug"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Debug(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static void DebugFormat(string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Log a message object with the <see cref="F:log4net.Core.Level.Error"/> level including
		/// the stack trace of the <see cref="T:System.Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// See the <see cref="M:log4net.ILog.Error(System.Object)"/> form for more detailed information.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
		public static void Error(object message, Exception exception)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a message object with the <see cref="F:log4net.Core.Level.Error"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Error"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// This method first checks if this logger is <c>ERROR</c>
		/// enabled by comparing the level of this logger with the
		/// <see cref="F:log4net.Core.Level.Error"/> level. If this logger is
		/// <c>ERROR</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
		/// proceeds to call all the registered appenders in this logger
		/// and also higher in the hierarchy depending on the value of the
		/// additivity flag.
		/// </para>
		/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
		/// to this method will print the name of the <see cref="T:System.Exception"/>
		/// but no stack trace. To print a stack trace use the
		/// <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/> form instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
		public static void Error(object message)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
		/// </summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
		public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <param name="arg2">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
		public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
		public static void ErrorFormat(string format, object arg0, object arg1)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
		public static void ErrorFormat(string format, object arg0)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Error"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Error(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsErrorEnabled"/>
		public static void ErrorFormat(string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Log a message object with the <see cref="F:log4net.Core.Level.Fatal"/> level including
		/// the stack trace of the <see cref="T:System.Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// See the <see cref="M:log4net.ILog.Fatal(System.Object)"/> form for more detailed information.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
		public static void Fatal(object message, Exception exception)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Log a message object with the <see cref="F:log4net.Core.Level.Fatal"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Fatal"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// This method first checks if this logger is <c>FATAL</c>
		/// enabled by comparing the level of this logger with the
		/// <see cref="F:log4net.Core.Level.Fatal"/> level. If this logger is
		/// <c>FATAL</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
		/// proceeds to call all the registered appenders in this logger
		/// and also higher in the hierarchy depending on the value of the
		/// additivity flag.
		/// </para>
		/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
		/// to this method will print the name of the <see cref="T:System.Exception"/>
		/// but no stack trace. To print a stack trace use the
		/// <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/> form instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
		public static void Fatal(object message)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
		/// </summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
		public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <param name="arg2">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
		public static void FatalFormat(string format, object arg0, object arg1, object arg2)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
		public static void FatalFormat(string format, object arg0, object arg1)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
		public static void FatalFormat(string format, object arg0)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Fatal"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Fatal(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsFatalEnabled"/>
		public static void FatalFormat(string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a message object with the <c>INFO</c> level including
		/// the stack trace of the <see cref="T:System.Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// See the <see cref="M:log4net.ILog.Info(System.Object)"/> form for more detailed information.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
		public static void Info(object message, Exception exception)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a message object with the <see cref="F:log4net.Core.Level.Info"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Info"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// This method first checks if this logger is <c>INFO</c>
		/// enabled by comparing the level of this logger with the
		/// <see cref="F:log4net.Core.Level.Info"/> level. If this logger is
		/// <c>INFO</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
		/// proceeds to call all the registered appenders in this logger
		/// and also higher in the hierarchy depending on the value of the
		/// additivity flag.
		/// </para>
		/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
		/// to this method will print the name of the <see cref="T:System.Exception"/>
		/// but no stack trace. To print a stack trace use the
		/// <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/> form instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
		public static void Info(object message)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
		/// </summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
		public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <param name="arg2">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
		public static void InfoFormat(string format, object arg0, object arg1, object arg2)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
		public static void InfoFormat(string format, object arg0, object arg1)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
		public static void InfoFormat(string format, object arg0)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Info"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Info(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsInfoEnabled"/>
		public static void InfoFormat(string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Debug"/> level.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Debug"/> events, <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// 	<para>
		/// This function is intended to lessen the computational cost of
		/// disabled log debug statements.
		/// </para>
		/// 	<para> For some ILog interface <c>log</c>, when you write:</para>
		/// 	<code lang="C#">
		/// log.Debug("This is entry number: " + i );
		/// </code>
		/// 	<para>
		/// You incur the cost constructing the message, string construction and concatenation in
		/// this case, regardless of whether the message is logged or not.
		/// </para>
		/// 	<para>
		/// If you are worried about speed (who isn't), then you should write:
		/// </para>
		/// 	<code lang="C#">
		/// if (log.IsDebugEnabled)
		/// {
		/// log.Debug("This is entry number: " + i );
		/// }
		/// </code>
		/// 	<para>
		/// This way you will not incur the cost of parameter
		/// construction if debugging is disabled for <c>log</c>. On
		/// the other hand, if the <c>log</c> is debug enabled, you
		/// will incur the cost of evaluating whether the logger is debug
		/// enabled twice. Once in <see cref="P:log4net.ILog.IsDebugEnabled"/> and once in
		/// the <see cref="M:log4net.ILog.Debug(System.Object)"/>.  This is an insignificant overhead
		/// since evaluating a logger takes about 1% of the time it
		/// takes to actually log. This is the preferred style of logging.
		/// </para>
		/// 	<para>Alternatively if your logger is available statically then the is debug
		/// enabled state can be stored in a static variable like this:
		/// </para>
		/// 	<code lang="C#">
		/// private static readonly bool isDebugEnabled = log.IsDebugEnabled;
		/// </code>
		/// 	<para>
		/// Then when you come to log you can write:
		/// </para>
		/// 	<code lang="C#">
		/// if (isDebugEnabled)
		/// {
		/// log.Debug("This is entry number: " + i );
		/// }
		/// </code>
		/// 	<para>
		/// This way the debug enabled state is only queried once
		/// when the class is loaded. Using a <c>private static readonly</c>
		/// variable is the most efficient because it is a run time constant
		/// and can be heavily optimized by the JIT compiler.
		/// </para>
		/// 	<para>
		/// Of course if you use a static readonly variable to
		/// hold the enabled state of the logger then you cannot
		/// change the enabled state at runtime to vary the logging
		/// that is produced. You have to decide if you need absolute
		/// speed or runtime flexibility.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Debug(System.Object)"/>
		/// <seealso cref="M:log4net.ILog.DebugFormat(System.IFormatProvider,System.String,System.Object[])"/>
		public static bool IsDebugEnabled
		{
			get
			{
				throw GetPublicMethodInvokedException();
			}
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Error"/> level.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Error"/> events, <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Error(System.Object)"/>
		/// <seealso cref="M:log4net.ILog.ErrorFormat(System.IFormatProvider,System.String,System.Object[])"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static bool IsErrorEnabled
		{
			get
			{
				throw GetPublicMethodInvokedException();
			}
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Fatal"/> level.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Fatal"/> events, <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Fatal(System.Object)"/>
		/// <seealso cref="M:log4net.ILog.FatalFormat(System.IFormatProvider,System.String,System.Object[])"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static bool IsFatalEnabled
		{
			get
			{
				throw GetPublicMethodInvokedException();
			}
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Info"/> level.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Info"/> events, <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Info(System.Object)"/>
		/// <seealso cref="M:log4net.ILog.InfoFormat(System.IFormatProvider,System.String,System.Object[])"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static bool IsInfoEnabled
		{
			get
			{
				throw GetPublicMethodInvokedException();
			}
		}

		/// <summary>
		/// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Warn"/> level.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Warn"/> events, <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// For more information see <see cref="P:log4net.ILog.IsDebugEnabled"/>.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
		/// <seealso cref="M:log4net.ILog.WarnFormat(System.IFormatProvider,System.String,System.Object[])"/>
		/// <seealso cref="P:log4net.ILog.IsDebugEnabled"/>
		public static bool IsWarnEnabled
		{
			get
			{
				throw GetPublicMethodInvokedException();
			}
		}

		/// <summary>
		/// Log a message object with the <see cref="F:log4net.Core.Level.Warn"/> level including
		/// the stack trace of the <see cref="T:System.Exception"/> passed
		/// as a parameter.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// See the <see cref="M:log4net.ILog.Warn(System.Object)"/> form for more detailed information.
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
		public static void Warn(object message, Exception exception)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Log a message object with the <see cref="F:log4net.Core.Level.Warn"/> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Warn"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// This method first checks if this logger is <c>WARN</c>
		/// enabled by comparing the level of this logger with the
		/// <see cref="F:log4net.Core.Level.Warn"/> level. If this logger is
		/// <c>WARN</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then
		/// proceeds to call all the registered appenders in this logger
		/// and also higher in the hierarchy depending on the value of the
		/// additivity flag.
		/// </para>
		/// 	<para><b>WARNING</b> Note that passing an <see cref="T:System.Exception"/>
		/// to this method will print the name of the <see cref="T:System.Exception"/>
		/// but no stack trace. To print a stack trace use the
		/// <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/> form instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
		public static void Warn(object message)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
		/// </summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
		public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <param name="arg2">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
		public static void WarnFormat(string format, object arg0, object arg1, object arg2)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <param name="arg1">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
		public static void WarnFormat(string format, object arg0, object arg1)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="arg0">An Object to format</param>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object)"/>
		/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
		public static void WarnFormat(string format, object arg0)
		{
			throw GetPublicMethodInvokedException();
		}

		/// <summary>
		/// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Warn"/> level.</overloads>
		/// <remarks>
		/// 	<para>
		/// The message is formatted using the <c>String.Format</c> method. See
		/// <see cref="M:System.String.Format(System.String,System.Object[])"/> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// 	<para>
		/// This method does not take an <see cref="T:System.Exception"/> object to include in the
		/// log event. To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:log4net.ILog.Warn(System.Object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.ILog.Warn(System.Object,System.Exception)"/>
		/// <seealso cref="P:log4net.ILog.IsWarnEnabled"/>
		public static void WarnFormat(string format, params object[] args)
		{
			throw GetPublicMethodInvokedException();
		}
		#endregion
	}
}