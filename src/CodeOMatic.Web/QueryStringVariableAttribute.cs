using System;
using System.Web;
using System.Web.SessionState;
using System.Collections.Specialized;

namespace CodeOMatic.Web
{
	/// <summary>
	/// Implements an abstract property as a query string parameter.
	/// </summary>
	[Serializable]
	public sealed class QueryStringVariableAttribute : CollectionVariableAttribute
	{
		private static NameValueCollection QueryString
		{
			get
			{
				return GetQueryString();
			}
		}

		private static NameValueCollection GetQueryString()
		{
			if (HttpContext.Current == null || HttpContext.Current.Request == null)
			{
				throw new InvalidOperationException("This property should only be accessed in the context of an HTTP request.");
			}

			NameValueCollection queryString = HttpContext.Current.Request.QueryString;
			if (queryString == null)
			{
				throw new InvalidOperationException("There is no query string.");
			}
			return queryString;
		}

		///
		public override void OnExecution(PostSharp.Laos.MethodExecutionEventArgs eventArgs)
		{
			GetQueryString();
			base.OnExecution(eventArgs);
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <returns></returns>
		protected override object GetValue(object target)
		{
			return QueryString[Key] ?? CalculateDefaultValue(target);
		}

		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="target">The object on which the property should be set.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		protected override void SetValue(object target, object value, object defaultValue)
		{
			QueryString[Key] = value != null ? value.ToString() : null;
		}
	}
}