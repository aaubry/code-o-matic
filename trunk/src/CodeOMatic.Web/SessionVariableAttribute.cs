using System;
using System.Web;
using System.Web.SessionState;

namespace CodeOMatic.Web
{
	/// <summary>
	/// Implements an abstract property as a session variable.
	/// </summary>
	[Serializable]
	public sealed class SessionVariableAttribute : CollectionVariableAttribute
	{
		private static HttpSessionState Session
		{
			get
			{
				if (HttpContext.Current == null)
				{
					throw new InvalidOperationException("This property should only be accessed in the context of an HTTP request.");
				}

				HttpSessionState session = HttpContext.Current.Session;
				if (session == null)
				{
					throw new InvalidOperationException("There is no session.");
				}

				return session;
			}
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		/// <returns></returns>
		protected override object GetValue(object target, object defaultValue)
		{
			return Session[Key] ?? defaultValue;
		}

		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="target">The object on which the property should be set.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		protected override void SetValue(object target, object value, object defaultValue)
		{
			Session[Key] = value;
		}
	}
}