using System;
using System.Globalization;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Validates that the first parameter is equal to the second parameter.
	/// </summary>
	[Serializable]
	public sealed class EqualAttribute : ComparisonValidatorAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LessAttribute"/> class.
		/// </summary>
		/// <param name="firstParameter">The name of the first parameter.</param>
		/// <param name="secondParameter">The name of the second parameter.</param>
		public EqualAttribute(string firstParameter, string secondParameter)
			: base(firstParameter, secondParameter)
		{
		}

		/// <summary>
		/// Validates the parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="first">The first parameter.</param>
		/// <param name="second">The second parameter.</param>
		protected override void Validate(object target, object first, object second)
		{
			if (first == null && second != null || first != null && second == null)
			{
				Validate(target, -1);
			}
			else
			{
				base.Validate(target, first, second);
			}
		}

		/// <summary>
		/// Validates the result of the comparison of both parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="comparisonResult">The comparison result.</param>
		protected override void Validate(object target, int comparisonResult)
		{
			if (comparisonResult != 0)
			{
				ValidationFailed(string.Format(
					CultureInfo.InvariantCulture,
					"Parameter '{0}' should be equal to parameter '{1}'",
					FirstParameter,
					SecondParameter
				));
			}
		}
	}
}