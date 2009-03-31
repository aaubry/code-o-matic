using System;
using System.Globalization;
using PostSharp.Extensibility;
using PostSharp.CodeModel;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Validates that a parameter is in the specified interval
	/// </summary>
	[Serializable]
	public sealed class IntervalAttribute : SpecificExceptionParameterValidatorAttribute
	{
		private readonly object min;

		/// <summary>
		/// Gets the minimum value that the parameter can have.
		/// </summary>
		public object Min
		{
			get
			{
				return min;
			}
		}

		private BoundaryMode minMode;

		/// <summary>
		/// Gets or sets a value indicating whether the minimum value is inclusive or exclusive.
		/// </summary>
		public BoundaryMode MinMode
		{
			get
			{
				return minMode;
			}
			set
			{
				minMode = value;
			}
		}

		private readonly object max;

		/// <summary>
		/// Gets the maximum value that the parameter can have.
		/// </summary>
		public object Max
		{
			get
			{
				return max;
			}
		}

		private BoundaryMode maxMode;

		/// <summary>
		/// Gets or sets a value indicating whether the maximum value is inclusive or exclusive.
		/// </summary>
		public BoundaryMode MaxMode
		{
			get
			{
				return maxMode;
			}
			set
			{
				maxMode = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IntervalAttribute"/> class.
		/// </summary>
		/// <param name="min">The minimum value that the parameter can have.</param>
		/// <param name="max">The maximum value that the parameter can have.</param>
		public IntervalAttribute(object min, object max)
		{
			this.min = min;
			this.max = max;
		}

		/// <summary>
		/// Validates the parameter.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public override void Validate(object target, object value, string parameterName)
		{
			if (value != null)
			{
				IComparable comparable = (IComparable)value;
				ValidateBoundary(minMode, comparable.CompareTo(min), value, parameterName);
				ValidateBoundary(maxMode, -comparable.CompareTo(max), value, parameterName);
			}
		}

		private void ValidateBoundary(BoundaryMode comparisonMode, int comparisonResult, object parameterValue, string parameterName)
		{
			if (comparisonResult < 0 || (comparisonResult == 0 && comparisonMode == BoundaryMode.Exclusive))
			{
				ValidationFailed(null, parameterValue, parameterName);
			}
		}

		/// <summary>
		/// Creates the default exception for the validator.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="parameterName">Name of the parameter that is being validated.</param>
		/// <param name="parameterValue">The value of the parameter.</param>
		/// <returns></returns>
		protected override Exception CreateDefaultException(string errorMessage, string parameterName, object parameterValue)
		{
			if (errorMessage == null)
			{
				errorMessage = string.Format(
					CultureInfo.InvariantCulture,
					"The argument must be in the interval {0}{1}, {2}{3}",
					minMode == BoundaryMode.Inclusive ? '[' : ']',
					min,
					max,
					maxMode == BoundaryMode.Inclusive ? ']' : '['
				);
			}

			return new ArgumentOutOfRangeException(parameterName, parameterValue, errorMessage);
		}

		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on a specific parameter.
		/// </summary>
		/// <param name="parameter">The parameter on which the attribute is applied.</param>
		/// <param name="memberType">The type that will be validated by the attribute.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		public override void CompileTimeValidate(ParameterDeclaration parameter, Type memberType, IMessageSink messages)
		{
			base.CompileTimeValidate(parameter, memberType, messages);

			if (!typeof(IComparable).IsAssignableFrom(memberType))
			{
				messages.Write(new Message(
					SeverityType.Error,
					"IntervalAttribute_TypeNotSupported",
					string.Format(CultureInfo.InvariantCulture, "The type '{0}' does not implement IComparable.", memberType.Name),
					GetType().FullName
				));
			}
		}
	}
}