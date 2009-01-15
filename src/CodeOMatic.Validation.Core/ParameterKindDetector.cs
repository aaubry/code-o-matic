using System;
using System.Reflection;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation.Core
{
	internal static class ParameterKindDetector
	{
		private static readonly ParameterKinds[][] acceptedParameterKinds = {
			// void Validate(object value)
			// void Validate(ParameterType value)
			// static void Validate(object target, object value)
			// static void Validate(ValidatedType target, object value)
			// static void Validate(object target, ParameterType value)
			// static void Validate(ValidatedType target, ParameterType value)
			new[] { ParameterKinds.ValidatedType, ParameterKinds.ParameterType, ParameterKinds.None, ParameterKinds.None },
			
			// void Validate(object value, string parameterName)
			// void Validate(ParameterType value, string parameterName)
			// static void Validate(object target, object value, string parameterName)
			// static void Validate(ValidatedType target, object value, string parameterName)
			// static void Validate(object target, ParameterType value, string parameterName)
			// static void Validate(ValidatedType target, ParameterType value, string parameterName)
			new[] { ParameterKinds.ValidatedType, ParameterKinds.ParameterType, ParameterKinds.ParameterName, ParameterKinds.None },

			// static void Validate(object value)
			// static void Validate(ParameterType value)
			new[] { ParameterKinds.ParameterType, ParameterKinds.None, ParameterKinds.None, ParameterKinds.None },

			// static void Validate(object value, string parameterName)
			// static void Validate(ParameterType value, string parameterName)
			new[] { ParameterKinds.ParameterType, ParameterKinds.ParameterName, ParameterKinds.None, ParameterKinds.None },

			// void IValidator.Validate(object value)
			// void IValidator.Validate(ParameterType value)
			new[] { ParameterKinds.ValidatorAttribute, ParameterKinds.ParameterType, ParameterKinds.None, ParameterKinds.None },

			// void IValidator.Validate(object target, object value)
			// void IValidator.Validate(object ValidatedType, object value)
			// void IValidator.Validate(object target, ParameterType value)
			// void IValidator.Validate(object ValidatedType, ParameterType value)
			new[] { ParameterKinds.ValidatorAttribute, ParameterKinds.ValidatedType, ParameterKinds.ParameterType, ParameterKinds.None },

			// void IValidator.Validate(object target, object value, string parameterName)
			// void IValidator.Validate(object ValidatedType, object value, string parameterName)
			// void IValidator.Validate(object target, ParameterType value, string parameterName)
			// void IValidator.Validate(object ValidatedType, ParameterType value, string parameterName)
			new[] { ParameterKinds.ValidatorAttribute, ParameterKinds.ValidatedType, ParameterKinds.ParameterType, ParameterKinds.ParameterName },
		};

		/// <summary>
		/// Determines the parameters that must be passed to a validation method based on the validated type and parameter.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="parameterType">Type of the parameter.</param>
		/// <param name="validatedType">Type of the validated.</param>
		/// <returns></returns>
		public static ParameterKinds[] GetParameterKinds(MethodBase method, Type parameterType, Type validatedType)
		{
			const int maxParameterCount = 4;

			Type[] parameterTypes = new Type[maxParameterCount];

			int currentIndex = 0;
			if (!method.IsStatic)
			{
				parameterTypes[currentIndex++] = method.DeclaringType;
			}

			ParameterInfo[] methodParameters = method.GetParameters();
			if (methodParameters.Length <= maxParameterCount - currentIndex)
			{
				Array.ForEach(method.GetParameters(), argument => parameterTypes[currentIndex++] = argument.ParameterType);

				ParameterKinds[] parameterKinds = Array.ConvertAll(parameterTypes,
					type =>
					{
						ParameterKinds kind = ParameterKinds.None;
						if(typeof(IValidator).IsAssignableFrom(type))
						{
							kind |= ParameterKinds.ValidatorAttribute;
						}
						if(type == parameterType || type == typeof(object))
						{
							kind |= ParameterKinds.ParameterType;
						}
						if(type == validatedType || type == typeof(object))
						{
							kind |= ParameterKinds.ValidatedType;
						}
						if(type == typeof(string))
						{
							kind |= ParameterKinds.ParameterName;
						}
						return kind;
					}
				);

				foreach(var acceptedParameterKind in acceptedParameterKinds)
				{
					bool isAcceptableMethod = true;
					for(int i = 0; i < maxParameterCount; ++i)
					{
						bool accpetedIsNoneAndActualIsOther = acceptedParameterKind[i] == ParameterKinds.None && parameterKinds[i] != ParameterKinds.None;
						bool parameterIsUnacceptable = (acceptedParameterKind[i] & parameterKinds[i]) != acceptedParameterKind[i];

						if (accpetedIsNoneAndActualIsOther || parameterIsUnacceptable)
						{
							isAcceptableMethod = false;
							break;
						}
					}

					if(isAcceptableMethod)
					{
						return acceptedParameterKind;
					}
				}
			}
			return null;
		}
	}
}