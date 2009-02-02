using System;
using System.Globalization;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Validates that the first parameter is less than or equal to the second parameter.
	/// </summary>
	[Serializable]
	public sealed class LessOrEqualAttribute : ComparisonValidatorAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LessAttribute"/> class.
		/// </summary>
		/// <param name="firstParameter">The name of the first parameter.</param>
		/// <param name="secondParameter">The name of the second parameter.</param>
		public LessOrEqualAttribute(string firstParameter, string secondParameter)
			: base(firstParameter, secondParameter)
		{
		}

		/// <summary>
		/// Validates the result of the comparison of both parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="comparisonResult">The comparison result.</param>
		protected override void Validate(object target, int comparisonResult)
		{
			if (comparisonResult > 0)
			{
				ValidationFailed(string.Format(
					CultureInfo.InvariantCulture,
					"Parameter '{0}' should be less than or equal to parameter '{1}'",
					FirstParameter,
					SecondParameter
				));
			}
		}
	}
}