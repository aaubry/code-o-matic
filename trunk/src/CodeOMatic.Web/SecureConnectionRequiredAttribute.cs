using System;
using PostSharp.Laos;
using System.Web;
using System.Configuration;
using System.Security;

namespace CodeOMatic.Web
{
	/// <summary>
	/// Throws an exception if the method is not invoked using an secure (HTTPS) connection.
	/// </summary>
	/// <remarks>
	/// The security check can be disabled by setting a value to "false" in the appSettings
	/// section of the configuration file.
	/// </remarks>
	[Serializable]
	public sealed class SecureConnectionRequiredAttribute : OnMethodBoundaryAspect
	{
		private string appSettingsDisableKey = "SecureConnectionRequired";

		/// <summary>
		/// Gets or sets the name of the app settings key that can be used to disable the check.
		/// The default value of this property is "SecureConnectionRequired".
		/// </summary>
		public string AppSettingsDisableKey
		{
			get
			{
				return appSettingsDisableKey;
			}
			set
			{
				appSettingsDisableKey = value;
			}
		}

		/// <summary/>
		public override void OnEntry(MethodExecutionEventArgs eventArgs)
		{
			bool isSecureConnection = HttpContext.Current.Request.IsSecureConnection;
			if(!isSecureConnection)
			{
				bool secureConnectionIsRequired;
				if(!bool.TryParse(ConfigurationManager.AppSettings[appSettingsDisableKey], out secureConnectionIsRequired) || secureConnectionIsRequired)
				{
					throw new SecurityException("A secure connection is required to call this method.");
				}
			}
		}
	}
}