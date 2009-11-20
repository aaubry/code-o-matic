using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Reflection;
using log4net.Config;
using System.Diagnostics;
using System.Globalization;

namespace CodeOMatic.Logging
{
	/// <summary>
	/// Configures Log4Net based on a predefined configuration.
	/// </summary>
	public class SimpleLogConfigurator
	{
		/// <summary>
		/// Configures the log with configuration from Web.config.
		/// This method reads the following configuration settings:
		/// <list type="table">
		/// <listheader>
		/// <term>Name</term>
		/// <description>Description</description>
		/// </listheader>
		/// <item>
		/// <term>Log.Level</term>
		/// <description>The minimum level required to append messages to the log. Valid values are DEBUG, INFO, WARN, ERROR, FATAL, OFF.</description>
		/// </item>
		/// <item>
		/// <term>Log.FileDir</term>
		/// <description>The path of the directory where the log files will be written.</description>
		/// </item>
		/// <item>
		/// <term>Log.FileLevel</term>
		/// <description>The minimum level required to append messages to the file log. Valid values are DEBUG, INFO, WARN, ERROR, FATAL, OFF.</description>
		/// </item>
		/// <item>
		/// <term>Log.EmailLevel</term>
		/// <description>The minimum level required to send messages by email. Valid values are DEBUG, INFO, WARN, ERROR, FATAL, OFF.</description>
		/// </item>
		/// <item>
		/// <term>Log.MessageFormat"]</term>
		/// <description>The format of the log messages.</description>
		/// </item>
		/// <item>
		/// <term>Log.EmailFrom"]</term>
		/// <description>The sender of the emails.</description>
		/// </item>
		/// <item>
		/// <term>Log.EmailTo"]</term>
		/// <description>The recipient of the emails.</description>
		/// </item>
		/// <item>
		/// <term>Log.EmailServer"]</term>
		/// <description>The address of the email server.</description>
		/// </item>
		/// </list>
		/// </summary>
		public void Configure()
		{
			LoadConfiguration("StandardConfiguration", true);
		}

		/// <summary>
		/// Configures the logging to send every log message to the console.
		/// </summary>
		public void ConfigureToConsole()
		{
			LoadConfiguration("ConsoleConfiguration", false);
		}

		/// <summary>
		/// Customizes the standard configuration using the settings from AppSettings.
		/// </summary>
		/// <remarks>
		/// Override this method to obtain the configuration values from a different source
		/// and / or to add / remove appenders.
		/// </remarks>
		/// <param name="configuration">The configuration.</param>
		protected virtual void CustomizeConfiguration(XmlDocument configuration)
		{
			string applicationName = GetApplicationName();

			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'RollingFileAppender']/file/@value",
				GetLogFileName(applicationName)
			);

			SetAttributes(
				configuration,
				"/log4net/root/level/@value",
				GetLogLevel()
			);

			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'RollingFileAppender']/filter/levelMin/@value",
				GetLogFileLevel()
			);

			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/to/@value",
				GetLogEmailTo()
			);

			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/from/@value",
				GetLogEmailFrom()
			);
			
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/subject/@value",
				GetEmailSubject(applicationName)
			);
			
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/smtpHost/@value",
				GetLogEmailServer()
			);
			
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/evaluator/threshold/@value",
				GetLogEmailLevel()
			);

			string messageFormat = GetAppSettingsValue("Log.MessageFormat", null);
			if (messageFormat != null)
			{
				SetAttributes(
					configuration,
					"/log4net/appender/layout/conversionPattern/@value",
					messageFormat
				);
			}
		}

		/// <summary>
		/// Gets the log email level.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetLogEmailLevel()
		{
			return GetAppSettingsValue("Log.EmailLevel", "FATAL");
		}

		/// <summary>
		/// Gets the log email server.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetLogEmailServer()
		{
			return GetAppSettingsValue("Log.EmailServer");
		}

		/// <summary>
		/// Gets the email subject.
		/// </summary>
		/// <param name="applicationName">Name of the application.</param>
		/// <returns></returns>
		protected virtual string GetEmailSubject(string applicationName)
		{
			return string.Format(CultureInfo.InvariantCulture, "Error from the '{0}' application on server '{1}'", applicationName, Environment.MachineName);
		}

		/// <summary>
		/// Gets the log email recipient.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetLogEmailTo()
		{
			return GetAppSettingsValue("Log.EmailTo");
		}

		/// <summary>
		/// Gets the log email sender.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetLogEmailFrom()
		{
			return GetAppSettingsValue("Log.EmailFrom", GetAppSettingsValue("Log.EmailTo"));
		}

		/// <summary>
		/// Gets the log file level.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetLogFileLevel()
		{
			return GetAppSettingsValue("Log.FileLevel", "DEBUG");
		}

		/// <summary>
		/// Gets the log level.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetLogLevel()
		{
			return GetAppSettingsValue("Log.Level", "DEBUG");
		}

		/// <summary>
		/// Gets the name of the log file.
		/// </summary>
		/// <param name="applicationName">Name of the application.</param>
		/// <returns></returns>
		protected virtual string GetLogFileName(string applicationName)
		{
			return Path.Combine(GetLogFileDir(), applicationName + ".log.");
		}

		private static string GetAppSettingsValue(string key)
		{
			string value = ConfigurationManager.AppSettings[key];
			if (string.IsNullOrEmpty(value))
			{
				throw new ConfigurationErrorsException("The " + key + " setting is missing from appSettings, in Web.config.");
			}
			return value;
		}

		private static string GetAppSettingsValue(string key, string defaultValue)
		{
			return ConfigurationManager.AppSettings[key] ?? defaultValue;
		}

		/// <summary>
		/// Sets the value of one or more attributes.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="xpath">The xpath that selects the attribute(s).</param>
		/// <param name="value">The value of the attribute(s).</param>
		protected static void SetAttributes(XmlDocument configuration, string xpath, string value)
		{
			foreach(XmlAttribute attribute in configuration.SelectNodes(xpath))
			{
				attribute.Value = value;
			}
		}

		/// <summary>
		/// Gets the name of the application.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetApplicationName()
		{
			var stack = new StackTrace(false);
			foreach (var frame in stack.GetFrames())
			{
				var caller = frame.GetMethod();
				if(caller != null)
				{
					var callerType = caller.DeclaringType;
					if(!typeof(SimpleLogConfigurator).IsAssignableFrom(callerType))
					{
						return callerType.Assembly.GetName(false).Name;
					}
				}
			}
			return "Application";
		}

		private string GetLogFileDir()
		{
			string logPath = GetAppSettingsValue("Log.FileDir");

			if(GetLogFileLevel() != "OFF" && GetLogLevel() != "OFF")
			{
				if (!Directory.Exists(logPath))
				{
					throw new ConfigurationErrorsException("The directory indicated in the appSettings key Log.FileDir does not exist: " + logPath);
				}

				string writeTestFile = Path.Combine(logPath, Path.GetRandomFileName());
				try
				{
					var file = File.Create(writeTestFile);
					file.Close();
					File.Delete(writeTestFile);
				}
				catch (Exception err)
				{
					throw new ConfigurationErrorsException("The directory indicated in the appSettings key Log.FileDir is not writable: " + logPath, err);
				}
			}
			return logPath;
		}

		private void LoadConfiguration(string configurationFileName, bool customize)
		{
			var configuration = new XmlDocument();
			using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("CodeOMatic.Logging." + configurationFileName + ".xml"))
			{
				configuration.Load(resource);
			}

			if (customize)
			{
				CustomizeConfiguration(configuration);
			}

			XmlConfigurator.Configure(configuration.DocumentElement);
		}
	}
}
