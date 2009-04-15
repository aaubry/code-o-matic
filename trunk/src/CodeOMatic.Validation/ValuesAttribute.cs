using System;
using System.Globalization;
using System.Collections.Generic;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Validates that a parameter is one the specified values.
	/// </summary>
	[Serializable]
	[CLSCompliant(false)]
	public sealed class ValuesAttribute : SpecificExceptionParameterValidatorAttribute
	{
		private readonly object[] values;

		/// <summary>
		/// Gets the values.
		/// </summary>
		/// <value>The values.</value>
		public IList<object> Values
		{
			get
			{
				return values;
			}
		}

		private bool negate;

		/// <summary>
		/// Gets or sets a value indicating whether the validation is negated.
		/// </summary>
		public bool Negate
		{
			get
			{
				return negate;
			}
			set
			{
				negate = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValuesAttribute"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		public ValuesAttribute(params object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}

			if (values.Length == 0)
			{
				throw new ArgumentException("The list of values cannot be empty", "values");
			}

			this.values = Array.ConvertAll(values, value => ParseString(value));
		}

		/// <summary>
		/// Validates the specified value.
		/// </summary>
		/// <param name="target">The object on which the method is being called.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public override void Validate(object target, object value, string parameterName)
		{
			if(value != null)
			{
				bool isMatch = false;
				foreach(var acceptedValue in values)
				{
					if(value.Equals(acceptedValue))
					{
						isMatch = true;
						break;
					}
				}

				if(isMatch == negate)
				{
					ValidationFailed(
						string.Format(CultureInfo.InvariantCulture, "The parameter '{0}' is not in the list of allowed values. Current value was '{1}'.", parameterName, value),
						value,
						parameterName
					);
				}
			}
		}
	}
}
