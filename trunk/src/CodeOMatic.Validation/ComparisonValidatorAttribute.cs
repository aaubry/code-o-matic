using System;
using PostSharp.CodeModel;
using PostSharp.Extensibility;
using System.Globalization;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Performs validations by comparing two parameters.
	/// </summary>
	/// <remarks>
	/// At least one of the compared parameters must implement <see cref="IComparable"/>.
	/// If the first parameter is <see cref="IComparable"/>, its implemnentation will be used.
	/// Otherwise, the implementation of the second parameter will be used and the comparison result will be reversed.
	/// If the parameter that is <see cref="IComparable"/> is null, the comparison will not be performed.
	/// </remarks>
	[Serializable]
	public abstract class ComparisonValidatorAttribute : TwoParametersValidatorAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ComparisonValidatorAttribute"/> class.
		/// </summary>
		/// <param name="firstParameter">The name of the first parameter.</param>
		/// <param name="secondParameter">The name of the second parameter.</param>
		protected ComparisonValidatorAttribute(string firstParameter, string secondParameter)
			: base(firstParameter, secondParameter)
		{
		}

		private bool firstParameterIsComparable;

		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on specific method parameters.
		/// </summary>
		/// <param name="method">The method on which the attribute is applied.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <param name="firstParameter">The first parameter.</param>
		/// <param name="secondParameter">The second parameter.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		protected override void CompileTimeValidateParameters(MethodDefDeclaration method, IMessageSink messages, ParameterDeclaration firstParameter, ParameterDeclaration secondParameter)
		{
			base.CompileTimeValidateParameters(method, messages, firstParameter, secondParameter);

			if(typeof(IComparable).IsAssignableFrom(firstParameter.ParameterType.GetSystemType(null, null)))
			{
				firstParameterIsComparable = true;
			}
			else if(typeof(IComparable).IsAssignableFrom(firstParameter.ParameterType.GetSystemType(null, null)))
			{
				firstParameterIsComparable = false;
			}
			else
			{
				messages.Write(new Message(
					SeverityType.Error,
					"ComparisonValidatorAttribute_ParametersCantBeCompared",
					string.Format(CultureInfo.InvariantCulture, "None of the parameters '{0}' and '{1}' implements IComparable.", firstParameter.Name, secondParameter.Name),
					GetType().FullName
				));
			}
		}

		/// <summary>
		/// Validates the parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="first">The first parameter.</param>
		/// <param name="second">The second parameter.</param>
		protected override void Validate(object target, object first, object second)
		{
			if(firstParameterIsComparable)
			{
				if (first != null)
				{
					Validate(target, ((IComparable)first).CompareTo(second));
				}
			}
			else
			{
				if (second != null)
				{
					Validate(target, -((IComparable)second).CompareTo(first));
				}
			}
		}

		/// <summary>
		/// Validates the result of the comparison of both parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="comparisonResult">The comparison result.</param>
		protected abstract void Validate(object target, int comparisonResult);
	}
}