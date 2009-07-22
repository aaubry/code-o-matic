using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Reflection;
using log4net.Config;
using System.Diagnostics;

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
		/// <term>Log.FileDir</term>
		/// <description>The path of the directory where the log files will be written.</description>
		/// </item>
		/// <item>
		/// <term>Log.FileLevel</term>
		/// <description>The minimum level required to append messages to the log.</description>
		/// </item>
		/// <item>
		/// <term>Log.EmailLevel</term>
		/// <description>The minimum level required to send messages by email.</description>
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
		/// </item>		/// </list>
		/// <item>
		/// <term>Log.EmailServer"]</term>
		/// <description>The address of the email server.</description>
		/// </item>		/// </summary>
		public void Configure()
		{
			XmlDocument configuration = new XmlDocument();
			using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("CodeOMatic.Logging.StandardConfiguration.xml"))
			{
				configuration.Load(resource);
			}

			CustomizeConfiguration(configuration);

			XmlConfigurator.Configure(configuration.DocumentElement);
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
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'RollingFileAppender']/file/@value",
				Path.Combine(GetLogFileDir(), GetApplicationName() + ".log.")
			);
			
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'RollingFileAppender']/filter/levelMin/@value",
				GetAppSettingsValue("Log.FileLevel", "DEBUG")
			);

			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/from/@value",
				GetAppSettingsValue("Log.EmailFrom")
			);
			
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/to/@value",
				GetAppSettingsValue("Log.EmailTo")
			);
			
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/smtpHost/@value",
				GetAppSettingsValue("Log.EmailServer")
			);
			
			SetAttributes(
				configuration,
				"/log4net/appender[@name = 'SmtpAppender']/evaluator/threshold/@value",
				GetAppSettingsValue("Log.EmailLevel", "FATAL")
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
			StackTrace stack = new StackTrace(false);
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

		private static string GetLogFileDir()
		{
			string logPath = GetAppSettingsValue("Log.FileDir");
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
			catch(Exception err)
			{
				throw new ConfigurationErrorsException("The directory indicated in the appSettings key Log.FileDir is not writable: " + logPath, err);
			}
			return logPath;
		}
	}
}
