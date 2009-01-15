using System;

namespace CodeOMatic.Validation.Core
{
	[Flags]
	internal enum ParameterKinds
	{
		/// <summary>
		/// No parameter.
		/// </summary>
		None = 0,

		/// <summary>
		/// The type that contains the method being validated.
		/// </summary>
		ValidatedType = 1,

		/// <summary>
		/// The type of the parameter being validated.
		/// </summary>
		ParameterType = 2,

		/// <summary>
		/// The name of the parameter being validated.
		/// </summary>
		ParameterName = 4,

		/// <summary>
		/// The type of the validator attribute.
		/// </summary>
		ValidatorAttribute = 8,
	}
}