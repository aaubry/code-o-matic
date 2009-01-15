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
	}
}