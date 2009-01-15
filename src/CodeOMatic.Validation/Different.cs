using System;
using System.Globalization;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Validates that the first parameter is different from the second parameter.
	/// </summary>
	[Serializable]
	public sealed class DifferentAttribute : ComparisonValidatorAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LessAttribute"/> class.
		/// </summary>
		/// <param name="firstParameter">The name of the first parameter.</param>
		/// <param name="secondParameter">The name of the second parameter.</param>
		public DifferentAttribute(string firstParameter, string secondParameter)
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
			if (comparisonResult == 0)
			{
				ValidationFailed(string.Format(
					CultureInfo.InvariantCulture,
					"Parameter '{0}' should be different from parameter '{1}'",
					FirstParameter,
					SecondParameter
				));
			}
		}
	}
}