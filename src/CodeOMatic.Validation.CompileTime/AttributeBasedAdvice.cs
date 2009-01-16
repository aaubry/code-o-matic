using System;
using System.Globalization;
using System.Reflection;
using PostSharp.CodeModel;
using PostSharp.CodeWeaver;
using PostSharp.Collections;
using PostSharp.Extensibility;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation.CompileTime
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TValidatorAttribute">The type of the validator attribute.</typeparam>
	[CLSCompliant(false)]
	public abstract class AttributeBasedAdvice<TValidatorAttribute> : IAdvice, IDisposable where TValidatorAttribute : class, IValidator
	{
		private readonly CustomAttributeDeclaration attribute;
		private FieldDefDeclaration field;

		/// <summary>
		/// Gets the attribute that is being processed.
		/// </summary>
		protected CustomAttributeDeclaration Attribute
		{
			get
			{
				return attribute;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeBasedAdvice{TValidatorAttribute}"/> class.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		protected AttributeBasedAdvice(CustomAttributeDeclaration attribute)
		{
			this.attribute = attribute;
		}

		/// <summary>
		/// Gets the attribute field that will contain the deserialized attribute.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected FieldDefDeclaration GetAttributeField(WeavingContext context)
		{
			if (field == null)
			{
				field = new FieldDefDeclaration();
				string attributeName = ((NamedDeclaration)(((MemberRefDeclaration)(attribute.Constructor)).DeclaringType)).Name;
				field.Name = NameGenerator.Generate(attributeName.Replace('.', '_'));
				field.Attributes = FieldAttributes.Static | FieldAttributes.Private;
				field.FieldType = attribute.Constructor.DeclaringType;
				context.Method.DeclaringType.Fields.Add(field);
			}
			return field;
		}

		private TValidatorAttribute validatorInstance;

		/// <summary>
		/// Gets the validator that is being processed.
		/// </summary>
		protected TValidatorAttribute ValidatorInstance
		{
			get
			{
				if (validatorInstance == null)
				{
					validatorInstance = (TValidatorAttribute)attribute.ConstructRuntimeObject();
				}
				return validatorInstance;
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
		protected abstract int Priority
		{
			get;
		}

		/// <summary>
		/// Weave the current advice into a given join point, i.e. inject code.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="block">Block where the code has to be injected.</param>
		private void WeaveBeforeMethodBody(WeavingContext context, InstructionBlock block)
		{
			InstructionSequence entrySequence = context.Method.MethodBody.CreateInstructionSequence();
			block.AddInstructionSequence(entrySequence, NodePosition.Before, null);

			InstructionWriter writer = context.InstructionWriter;
			writer.AttachInstructionSequence(entrySequence);
			writer.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);

			Weave(context, writer);

			writer.DetachInstructionSequence();
		}

		/// <summary>
		/// Weave the current advice into a given join point, i.e. inject code.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="writer">The writer.</param>
		protected abstract void Weave(WeavingContext context, InstructionWriter writer);

		/// <summary>
		/// Weave the current advice into a given join point, i.e. inject code.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="block">Block where the code has to be injected.</param>
		private void WeaveBeforeStaticConstructor(WeavingContext context, InstructionBlock block)
		{
			CompileTimeValidate(ValidatorInstance, MessageSource.MessageSink);

			if (ValidatorInstance.IsPersistent)
			{
				InstructionSequence entrySequence = context.Method.MethodBody.CreateInstructionSequence();
				block.AddInstructionSequence(entrySequence, NodePosition.Before, null);
				context.InstructionWriter.AttachInstructionSequence(entrySequence);
				context.InstructionWriter.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);

				string serializedAttribute = InternalHelperMethods.SerializeToString(ValidatorInstance);
				context.InstructionWriter.EmitInstructionString(OpCodeNumber.Ldstr, new LiteralString(serializedAttribute));

				IMethod deserializeFromString =
					context.Method.Module.FindMethod(typeof(InternalHelperMethods).GetMethod("DeserializeFromString"),
						BindingOptions.Default);
				context.InstructionWriter.EmitInstructionMethod(OpCodeNumber.Call, deserializeFromString);

				context.InstructionWriter.EmitInstructionField(OpCodeNumber.Stsfld, GetAttributeField(context));

				context.InstructionWriter.DetachInstructionSequence();
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
		protected abstract void CompileTimeValidate(TValidatorAttribute attributeInstance, IMessageSink messages);

		#region IAdvice Members
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
		int IAdvice.Priority
		{
			get
			{
				return Priority;
			}
		}

		/// <summary>
		/// Determines whether the current advice requires to be woven on a given join point.
		/// </summary>
		/// <param name="context">Weaving context.</param>
		/// <returns>
		/// 	<b>true</b> if the <see cref="M:PostSharp.CodeWeaver.IAdvice.Weave(PostSharp.CodeWeaver.WeavingContext,PostSharp.CodeModel.InstructionBlock)"/> method should be called for
		/// this join point, otherwise <b>false</b>.
		/// </returns>
		/// <remarks>
		/// It is theoretically possible for this method to return always <b>false</b>,
		/// because the <see cref="M:PostSharp.CodeWeaver.IAdvice.Weave(PostSharp.CodeWeaver.WeavingContext,PostSharp.CodeModel.InstructionBlock)"/> method is not obliged to emit any code. However,
		/// restructuring the method body to make weaving possible is expensive, so the
		/// <see cref="M:PostSharp.CodeWeaver.IAdvice.RequiresWeave(PostSharp.CodeWeaver.WeavingContext)"/> method should be designed in order to minimize
		/// the number of useless calls to the <see cref="M:PostSharp.CodeWeaver.IAdvice.Weave(PostSharp.CodeWeaver.WeavingContext,PostSharp.CodeModel.InstructionBlock)"/> method.
		/// </remarks>
		bool IAdvice.RequiresWeave(WeavingContext context)
		{
			return RequiresWeave(context);
		}

		/// <summary>
		/// Determines whether the current advice requires to be woven on a given join point.
		/// </summary>
		/// <param name="context">Weaving context.</param>
		/// <returns>
		/// 	<b>true</b> if the <see cref="M:PostSharp.CodeWeaver.IAdvice.Weave(PostSharp.CodeWeaver.WeavingContext,PostSharp.CodeModel.InstructionBlock)"/> method should be called for
		/// this join point, otherwise <b>false</b>.
		/// </returns>
		/// <remarks>
		/// It is theoretically possible for this method to return always <b>false</b>,
		/// because the <see cref="M:PostSharp.CodeWeaver.IAdvice.Weave(PostSharp.CodeWeaver.WeavingContext,PostSharp.CodeModel.InstructionBlock)"/> method is not obliged to emit any code. However,
		/// restructuring the method body to make weaving possible is expensive, so the
		/// <see cref="M:PostSharp.CodeWeaver.IAdvice.RequiresWeave(PostSharp.CodeWeaver.WeavingContext)"/> method should be designed in order to minimize
		/// the number of useless calls to the <see cref="M:PostSharp.CodeWeaver.IAdvice.Weave(PostSharp.CodeWeaver.WeavingContext,PostSharp.CodeModel.InstructionBlock)"/> method.
		/// </remarks>
		protected virtual bool RequiresWeave(WeavingContext context)
		{
			return context.JoinPoint.JoinPointKind == JoinPointKinds.BeforeMethodBody;
		}

		/// <summary>
		/// Weave the current advice into a given join point, i.e. inject code.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="block">Block where the code has to be injected.</param>
		void IAdvice.Weave(WeavingContext context, InstructionBlock block)
		{
			if (context.Method.Name == ".cctor")
			{
				WeaveBeforeStaticConstructor(context, block);
			}
			else
			{
				WeaveBeforeMethodBody(context, block);
			}
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				field.Dispose();
				attribute.Dispose();
			}
			// free native resources if there are any.
		}
		#endregion
	}
}