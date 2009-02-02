using System;
using System.Diagnostics;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using PostSharp.Extensibility;
using System.Reflection;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation.CompileTime
{
	/// <summary>
	/// 
	/// </summary>
	[CLSCompliant(false)]
	public class ParameterProcessorAdvice : AttributeBasedAdvice<IParameterValidator>
	{
		private readonly int parameterIndex;
		private readonly ParameterDeclaration parameter;

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterProcessorAdvice"/> class.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="parameterIndex">Index of the parameter.</param>
		public ParameterProcessorAdvice(CustomAttributeDeclaration attribute, ParameterDeclaration parameter, int parameterIndex)
			: base(attribute)
		{
			this.parameter = parameter;
			this.parameterIndex = parameterIndex;
		}

		/// <summary>
		/// Gets the advice priority.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// This influences the order in which advices are applied to join points, in case
		/// that many advices are applied to the same join point. In join point of type
		/// <i>before</i> or <i>instead of</i>, advices are injected with <i>direct</i> order of priority.
		/// In join point of type <i>after</i>, advices are injected with <i>inverse</i> order of priority.
		/// </remarks>
		protected override int Priority
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on a specific method.
		/// </summary>
		/// <param name="attributeInstance">The attribute instance.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		protected override void CompileTimeValidate(IParameterValidator attributeInstance, IMessageSink messages)
		{
			attributeInstance.CompileTimeValidate(parameter, MessageSource.MessageSink);
		}

		/// <summary>
		/// Weave the current advice into a given join point, i.e. inject code.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="writer">The writer.</param>
		protected override void Weave(WeavingContext context, InstructionWriter writer)
		{
			MethodBase validationMethod = ValidatorInstance.GetValidationMethod(parameter);

			Type parameterType = parameter.ParameterType.GetSystemType(null, null);
			Type validatedType = parameter.Parent.DeclaringType.GetSystemType(null, null);

			int validationParameterIndex = validationMethod.IsStatic ? 0 : -1;

			ParameterKinds[] parameterKinds = ParameterKindDetector.GetParameterKinds(validationMethod, parameterType, validatedType);
			Debug.Assert(parameterKinds != null);

			foreach (var kind in parameterKinds)
			{
				switch (kind)
				{
					case ParameterKinds.None:
						// Do nothing
						break;

					case ParameterKinds.ValidatorAttribute:
						// If the method belongs to the validator attribute, we push a reference to it.
						writer.EmitInstructionField(OpCodeNumber.Ldsfld, GetAttributeField(context));
						break;

					case ParameterKinds.ValidatedType:
						// If the method to validate is static, we push null for the "target" parameter. Otherwise, we push the object.
						writer.EmitInstruction(context.Method.IsStatic ? OpCodeNumber.Ldnull : OpCodeNumber.Ldarg_0);
						break;

					case ParameterKinds.ParameterType:
						// Push the value of the parameter that is being validated.
						writer.EmitInstructionInt32(OpCodeNumber.Ldarg, context.Method.IsStatic ? parameterIndex : parameterIndex + 1);

						// Box the value if it must be converted from value-type to object.
						bool validatedParameterIsValueType = parameter.ParameterType.BelongsToClassification(TypeClassifications.ValueType);
						bool validationParameterIsValueType = validationMethod.GetParameters()[validationParameterIndex].ParameterType.IsValueType;
						if (validatedParameterIsValueType && !validationParameterIsValueType)
						{
							writer.EmitInstructionType(OpCodeNumber.Box, parameter.ParameterType);
						}
						break;

					case ParameterKinds.ParameterName:
						// If the validation method accepts the name of the parameter, push it also.
						writer.EmitInstructionString(OpCodeNumber.Ldstr, parameter.Name);
						break;

					default:
						throw new InvalidOperationException();
				}
				++validationParameterIndex;
			}

			// We can finally call the method.
			IMethod validate = context.Method.Module.FindMethod(validationMethod, BindingOptions.Default);
			if (validationMethod.IsStatic)
			{
				writer.EmitInstructionMethod(OpCodeNumber.Call, validate);
			}
			else
			{
				writer.EmitInstructionMethod(OpCodeNumber.Callvirt, validate);
			}
		}
	}
}