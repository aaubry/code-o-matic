using System;
using PostSharp.Extensibility;

namespace CodeOMatic.Validation.Core
{
	/// <summary>
	/// Base class for validating parameters.
	/// </summary>
	[Serializable]
	public abstract class ValidatorAttribute : Attribute, IRequirePostSharp
	{
		#region IRequirePostSharp Members
		/// <summary>
		/// Gets the post sharp requirements.
		/// </summary>
		/// <returns></returns>
		PostSharpRequirements IRequirePostSharp.GetPostSharpRequirements()
		{
			return GetPostSharpRequirements();
		}
		#endregion

		/// <summary>
		/// Gets the post sharp requirements.
		/// </summary>
		/// <returns></returns>
		protected virtual PostSharpRequirements GetPostSharpRequirements()
		{
			PostSharpRequirements requirements = new PostSharpRequirements();
			requirements.Tasks.Add("ParameterProcessor");
			return requirements;
		}

		/// <summary>
		/// If the <paramref name="value"/> parameter is a string, converts it to object using the <see cref="StringConverter"/> class.
		/// Otherwise, returns the value of the parameter.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
        protected static object ParseString(object value)
		{
			string text = value as string;
			return text != null ? StringConverter.Convert(text) : value;
		}
	}
}