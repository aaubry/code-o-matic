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
				return GetSession();
			}
		}

		private static HttpSessionState GetSession()
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

		///
		public override void OnExecution(PostSharp.Laos.MethodExecutionEventArgs eventArgs)
		{
			GetSession();
			base.OnExecution(eventArgs);
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <returns></returns>
		protected override object GetValue(object target)
		{
			return Session[Key] ?? CalculateDefaultValue(target);
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