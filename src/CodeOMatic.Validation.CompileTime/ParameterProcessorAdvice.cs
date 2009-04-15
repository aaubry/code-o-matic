using System;
using System.Diagnostics;
using System.Globalization;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using PostSharp.Extensibility;
using System.Reflection;
using CodeOMatic.Validation.Core;
using System.Collections;
using CodeOMatic.Validation.CompileTime.Parser;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PostSharp.Collections;

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

		#region MemberTypeVisitor
		private abstract class MemberTypeVisitor : ISelectorVisitor
		{
			private Type currentType;

			public Type CurrentType
			{
				get
				{
					return currentType;
				}
			}

			public MemberTypeVisitor(Type currentType)
			{
				this.currentType = currentType;
			}

			#region ISelectorVisitor Members
			void ISelectorVisitor.Visit(MemberSelectorPart part)
			{
				var property = currentType.GetProperty(part.MemberName, BindingFlags.Public | BindingFlags.Instance);
				if (property != null)
				{
					currentType = property.PropertyType;
					Visit(property);
					return;
				}

				var field = currentType.GetField(part.MemberName, BindingFlags.Public | BindingFlags.Instance);
				if (field != null)
				{
					currentType = field.FieldType;
					Visit(field);
					return;
				}

				VisitInvalid(part);
			}

			protected virtual void VisitInvalid(MemberSelectorPart part)
			{
				throw new InvalidOperationException("This should never occur.");
			}

			protected virtual void VisitInvalid(IterationSelectorPart part)
			{
				throw new InvalidOperationException("This should never occur.");
			}

			protected virtual void Visit(FieldInfo field)
			{
				// Nothing to do.
			}

			protected virtual void Visit(PropertyInfo property)
			{
				// Nothing to do.
			}

			protected virtual void VisitGenericEnumerable(Type interfaceType)
			{
				// Nothing to do.
			}

			protected virtual void VisitEnumerable()
			{
				// Nothing to do.
			}

			void ISelectorVisitor.Visit(IterationSelectorPart part)
			{
				Type genericEnumerable = null;
				Type enumerable = null;

				foreach (var interfaceType in EnumerateInterfaces(currentType))
				{
					if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
					{
						genericEnumerable = interfaceType;
					}
					else if (interfaceType == typeof(IEnumerable))
					{
						enumerable = interfaceType;
					}
				}

				if (genericEnumerable != null)
				{
					currentType = genericEnumerable.GetGenericArguments()[0];
					VisitGenericEnumerable(genericEnumerable);
				}
				else if (enumerable != null)
				{
					currentType = typeof(object);
					VisitEnumerable();
				}
				else
				{
					VisitInvalid(part);
				}
			}

			private IEnumerable<Type> EnumerateInterfaces(Type currentType)
			{
				if (currentType.IsInterface)
				{
					yield return currentType;
				}
				foreach (var interfaceType in currentType.GetInterfaces())
				{
					yield return interfaceType;
				}
			}
			#endregion
		}
		#endregion

		#region SelectorValidatorVisitor
		private sealed class SelectorValidatorVisitor : MemberTypeVisitor
		{
			private readonly Action<string> onError;
			private bool isValid = true;

			public bool IsValid
			{
				get
				{
					return isValid;
				}
			}

			public SelectorValidatorVisitor(Type currentType, Action<string> onError)
				: base(currentType)
			{
				this.onError = onError;
			}

			protected override void Visit(PropertyInfo property)
			{
				if (!property.CanRead)
				{
					isValid = false;
					onError(string.Format(CultureInfo.InvariantCulture, "The property '{0}' is not readable.", property.Name));
				}
				else if (property.GetIndexParameters().Length > 0)
				{
					isValid = false;
					onError(string.Format(CultureInfo.InvariantCulture, "The property '{0}' cannot be validated because it is an indexed property.", property.Name));
				}
				else if (property.GetGetMethod() == null)
				{
					isValid = false;
					onError(string.Format(CultureInfo.InvariantCulture, "The property '{0}' does not have a public getter.", property.Name));
				}
			}

			protected override void VisitInvalid(MemberSelectorPart part)
			{
				isValid = false;
				onError(string.Format(CultureInfo.InvariantCulture, "Could not find a public property or field named '{0}' on type '{1}.", part.MemberName, CurrentType.FullName));
			}

			protected override void VisitInvalid(IterationSelectorPart part)
			{
				onError(string.Format(CultureInfo.InvariantCulture, "Type '{0}' is neither IEnumerable<T> not IEnumerable.", CurrentType.FullName));
			}
		}
		#endregion

		#region MemberEmitterVisitor
		private sealed class MemberEmitterVisitor : MemberTypeVisitor
		{
			private readonly WeavingContext context;
			private readonly InstructionWriter writer;

			public MemberEmitterVisitor(Type currentType, WeavingContext context, InstructionWriter writer)
				: base(currentType)
			{
				this.context = context;
				this.writer = writer;
			}

			protected override void Visit(FieldInfo field)
			{
				ModuleDeclaration module = context.Method.Module;
				writer.EmitInstructionField(OpCodeNumber.Ldfld, module.FindField(field, BindingOptions.Default));
			}

			protected override void Visit(PropertyInfo property)
			{
				ModuleDeclaration module = context.Method.Module;
				var method = module.FindMethod(property.GetGetMethod(), BindingOptions.Default);
				writer.EmitInstructionMethod(OpCodeNumber.Callvirt, method);
			}

			protected override void VisitGenericEnumerable(Type interfaceType)
			{
				ModuleDeclaration module = context.Method.Module;
				Type genericEnumeratorType = typeof(IEnumerator<>).MakeGenericType(interfaceType.GetGenericArguments());
				ITypeSignature enumeratorType = module.FindType(genericEnumeratorType, BindingOptions.RequireGenericInstance);
				IMethod getEnumerator = module.FindMethod(interfaceType.GetMethod("GetEnumerator"), BindingOptions.RequireGenericInstance);
				IMethod getCurrent = module.FindMethod(genericEnumeratorType.GetProperty("Current").GetGetMethod(), BindingOptions.RequireGenericInstance);
				EmitEnumeration(enumeratorType, getEnumerator, getCurrent);
			}

			protected override void VisitEnumerable()
			{
				ModuleDeclaration module = context.Method.Module;
				ITypeSignature enumeratorType = module.FindType(typeof(IEnumerator), BindingOptions.Default);
				IMethod getEnumerator = module.FindMethod(typeof(IEnumerable).GetMethod("GetEnumerator"), BindingOptions.Default);
				IMethod getCurrent = module.FindMethod(typeof(IEnumerator).GetProperty("Current").GetGetMethod(), BindingOptions.Default);
				EmitEnumeration(enumeratorType, getEnumerator, getCurrent);
			}

			private void EmitEnumeration(ITypeSignature enumeratorType, IMethod getEnumerator, IMethod getCurrent)
			{
				ModuleDeclaration module = context.Method.Module;
				context.Method.MethodBody.InitLocalVariables = true;

				var beforeLoop = writer.CurrentInstructionSequence;

				var loopHeader = writer.MethodBody.CreateInstructionSequence();
				beforeLoop.ParentInstructionBlock.AddInstructionSequence(loopHeader, NodePosition.After, beforeLoop);

				var loop = writer.MethodBody.CreateInstructionSequence();
				beforeLoop.ParentInstructionBlock.AddInstructionSequence(loop, NodePosition.After, loopHeader);

				var loopControl = writer.MethodBody.CreateInstructionSequence();
				beforeLoop.ParentInstructionBlock.AddInstructionSequence(loopControl, NodePosition.After, loop);

				var enumerator = beforeLoop.ParentInstructionBlock.DefineLocalVariable(enumeratorType, NameGenerator.Generate("enum"));

				writer.EmitInstructionMethod(OpCodeNumber.Callvirt, getEnumerator);
				writer.EmitInstructionLocalVariable(OpCodeNumber.Stloc, enumerator);

				writer.EmitBranchingInstruction(OpCodeNumber.Br, loopControl);
				writer.DetachInstructionSequence();

				writer.AttachInstructionSequence(loopHeader);
				writer.EmitInstructionLocalVariable(OpCodeNumber.Ldloc, enumerator);
				
				writer.EmitInstructionMethod(OpCodeNumber.Callvirt, getCurrent);

				writer.DetachInstructionSequence();

				writer.AttachInstructionSequence(loopControl);
				writer.EmitInstructionLocalVariable(OpCodeNumber.Ldloc, enumerator);
				IMethod moveNext = module.FindMethod(typeof(IEnumerator).GetMethod("MoveNext"), BindingOptions.Default);
				writer.EmitInstructionMethod(OpCodeNumber.Callvirt, moveNext);
				writer.EmitBranchingInstruction(OpCodeNumber.Brtrue, loopHeader);
				writer.DetachInstructionSequence();

				writer.AttachInstructionSequence(loop);
			}
		}
		#endregion

		#region SelectorToStringVisitor
		private class SelectorToStringVisitor : ISelectorVisitor
		{
			private readonly StringBuilder buffer;

			public SelectorToStringVisitor(string parameterName)
			{
				buffer = new StringBuilder(parameterName);
			}

			/// <summary>
			/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
			/// </returns>
			public override string ToString()
			{
				return buffer.ToString();
			}

			#region ISelectorVisitor Members
			void ISelectorVisitor.Visit(MemberSelectorPart part)
			{
				buffer.Append('.');
				buffer.Append(part.MemberName);
			}

			void ISelectorVisitor.Visit(IterationSelectorPart part)
			{
				buffer.Append('*');
			}
			#endregion
		}
		#endregion

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
			string selectorsText = attributeInstance.Selectors ?? string.Empty;

			SelectorParser parser = new SelectorParser(new SelectorScanner(new MemoryStream(Encoding.UTF8.GetBytes(selectorsText))));
			parser.errors.errorStream = new StringWriter(CultureInfo.InvariantCulture);
			IEnumerable<MemberSelector> memberSelectors = parser.Parse();
			if (parser.errors.count > 0)
			{
				messages.Write(new Message(
					SeverityType.Error,
					"ParameterProcessorAdvice_SelectorParsingError",
					string.Format(
						CultureInfo.InvariantCulture,
						"Error(s) parsing the selector '{0}':\n{1}",
						selectorsText,
						parser.errors.errorStream
					),
					GetType().FullName
				));
			}

			foreach (var selector in memberSelectors)
			{
				var visitor = new SelectorValidatorVisitor(parameter.ParameterType.GetSystemType(null, null),
					delegate(string message)
					{
						messages.Write(new Message(
							SeverityType.Error,
							"ParameterProcessorAdvice_ErrorInSelector",
							string.Format(CultureInfo.InvariantCulture, "Error in selector '{0}': {1}", selectorsText, message),
							GetType().FullName
						));
					}
				);

				selector.Accept(visitor);

				if (visitor.IsValid)
				{
					attributeInstance.CompileTimeValidate(parameter, visitor.CurrentType, MessageSource.MessageSink);
				}
			}
		}

		private static readonly IEnumerable<MemberSelector> defaultSelectors = new[] { new MemberSelector(new SelectorPart[0]) };

		/// <summary>
		/// Weave the current advice into a given join point, i.e. inject code.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="writer">The writer.</param>
		protected override void Weave(WeavingContext context, InstructionWriter writer)
		{
			IEnumerable<MemberSelector> selectors;
			string selectorsText = ValidatorInstance.Selectors;

			if (string.IsNullOrEmpty(selectorsText))
			{
				selectors = defaultSelectors;
			}
			else
			{
				SelectorParser parser = new SelectorParser(new SelectorScanner(new MemoryStream(Encoding.UTF8.GetBytes(selectorsText))));
				parser.errors.errorStream = new StringWriter(CultureInfo.InvariantCulture);
				selectors = parser.Parse();
				if (parser.errors.count > 0)
				{
					return;
				}
			}

			foreach (var selector in selectors)
			{
				// Push the value of the parameter that is being validated.
				writer.EmitInstructionInt32(OpCodeNumber.Ldarg, parameterIndex + (context.Method.IsStatic ? 0 : 1));

				var visitor = new MemberEmitterVisitor(parameter.ParameterType.GetSystemType(null, null), context, writer);
				selector.Accept(visitor);

				var validatedType = context.Method.Module.FindType(visitor.CurrentType, visitor.CurrentType.IsGenericType ? BindingOptions.RequireGenericInstance : BindingOptions.Default);
				var value = writer.CurrentInstructionSequence.ParentInstructionBlock.DefineLocalVariable(validatedType, NameGenerator.Generate("value"));
				writer.EmitInstructionLocalVariable(OpCodeNumber.Stloc, value);

				MethodBase validationMethod = ValidatorInstance.GetValidationMethod(parameter, visitor.CurrentType);

				int validationParameterIndex = validationMethod.IsStatic ? 0 : -1;

				ParameterKinds[] parameterKinds = ParameterKindDetector.GetParameterKinds(validationMethod, visitor.CurrentType, parameter.Parent.DeclaringType.GetSystemType(null, null));
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
							// Recover the value to validate from the local variable.
							writer.EmitInstructionLocalVariable(OpCodeNumber.Ldloc, value);

							// Box the value if it must be converted from value-type to object.
							bool validatedParameterIsValueType = visitor.CurrentType.IsValueType;
							bool validationParameterIsValueType = validationMethod.GetParameters()[validationParameterIndex].ParameterType.IsValueType;
							if (validatedParameterIsValueType && !validationParameterIsValueType)
							{
								writer.EmitInstructionType(OpCodeNumber.Box, parameter.ParameterType);
							}
							break;

						case ParameterKinds.ParameterName:
							// If the validation method accepts the name of the parameter, push it also.
							var toString = new SelectorToStringVisitor(parameter.Name);
							selector.Accept(toString);
							writer.EmitInstructionString(OpCodeNumber.Ldstr, toString.ToString());
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
}