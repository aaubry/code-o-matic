using System;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using PostSharp.Extensibility;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation.CompileTime
{
	[CLSCompliant(false)]
	public class MethodProcessorAdvice : AttributeBasedAdvice<MethodValidatorAttribute>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodProcessorAdvice"/> class.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		public MethodProcessorAdvice(CustomAttributeDeclaration attribute)
			: base(attribute)
		{
		}

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>The method.</value>
		private MethodDefDeclaration Method
		{
			get
			{
				return (MethodDefDeclaration)Attribute.Parent;
			}
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
		/// Weave the current advice into a given join point, i.e. inject code.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="writer">The writer.</param>
		protected override void Weave(WeavingContext context, InstructionWriter writer)
		{
			writer.EmitInstructionField(OpCodeNumber.Ldsfld, GetAttributeField(context));

			if (context.Method.IsStatic)
			{
				writer.EmitInstruction(OpCodeNumber.Ldnull);
			}
			else
			{
				writer.EmitInstruction(OpCodeNumber.Ldarg_0);
			}

			//writer.EmitInstructionMethod(OpCodeNumber.Call, context.Method.Module.FindMethod(typeof(InternalHelperMethods).GetMethod("CreateParameterCollection"), BindingOptions.Default));

			//IMethod add = context.Method.Module.FindMethod(typeof(ParameterDictionary).GetMethod("Add", new[] { typeof(string), typeof(object) }), BindingOptions.Default);

			//int parameterIndex = 0;
			//foreach (var parameter in Method.Parameters)
			//{
			//    writer.EmitInstruction(OpCodeNumber.Dup);

			//    writer.EmitInstructionString(OpCodeNumber.Ldstr, new LiteralString(parameter.Name));
			//    writer.EmitInstructionInt32(OpCodeNumber.Ldarg, parameterIndex++);

			//    writer.EmitInstructionMethod(OpCodeNumber.Callvirt, add);
			//}

			//writer.EmitInstruction(OpCodeNumber.Ldnull);
			writer.EmitInstructionInt32(OpCodeNumber.Ldloc, writer.CurrentInstructionSequence.MethodBody.LocalVariableCount);

			IMethod validate = context.Method.Module.FindMethod(typeof(MethodValidatorAttribute).GetMethod("Validate"), BindingOptions.Default);
			writer.EmitInstructionMethod(OpCodeNumber.Callvirt, validate);
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
		protected override void CompileTimeValidate(MethodValidatorAttribute attributeInstance, IMessageSink messages)
		{
			attributeInstance.CompileTimeValidate(Method, MessageSource.MessageSink);
		}
	}
}