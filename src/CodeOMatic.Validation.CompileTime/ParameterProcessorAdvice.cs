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
		private class MemberTypeVisitor : ISelectorVisitor
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
				if(property != null)
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

			void ISelectorVisitor.Visit(IterationSelectorPart part)
			{
				Type genericEnumerable = null;
				Type enumerable = null;
				foreach (var interfaceType in currentType.GetInterfaces())
				{
					if(interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
					{
						genericEnumerable = interfaceType;
					}
					else if(interfaceType == typeof(IEnumerable))
					{
						enumerable = interfaceType;
					}
				}

				if(genericEnumerable != null)
				{
					currentType = genericEnumerable.GetGenericArguments()[0];
				}
				else if(enumerable != null)
				{
					currentType = typeof(object);
				}
				else
				{
					VisitInvalid(part);
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

		#region MemberTypeVisitor
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
				writer.EmitInstructionInt32(OpCodeNumber.Ldfld, field.MetadataToken);
			}

			protected override void Visit(PropertyInfo property)
			{
				writer.EmitInstructionInt32(OpCodeNumber.Callvirt, property.GetGetMethod().MetadataToken);
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

		#region SelectorInfo
		private struct SelectorInfo
		{
			private readonly MemberSelector selector;

			public MemberSelector Selector
			{
				get
				{
					return selector;
				}
			}

			private readonly Type memberType;

			public Type MemberType
			{
				get
				{
					return memberType;
				}
			}

			public SelectorInfo(MemberSelector selector, Type memberType)
			{
				this.selector = selector;
				this.memberType = memberType;
			}
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
			string selectorsText = attributeInstance.Selectors;
			if (!string.IsNullOrEmpty(selectorsText))
			{
				SelectorParser parser = new SelectorParser(new SelectorScanner(new MemoryStream(Encoding.UTF8.GetBytes(selectorsText))));
				parser.errors.errorStream = new StringWriter();
				IEnumerable<MemberSelector> memberSelectors = parser.Parse();
				if(parser.errors.count > 0)
				{
					messages.Write(new Message(
						SeverityType.Error,
						"ParameterProcessorAdvice_SelectorParsingError",
						string.Format(CultureInfo.InvariantCulture,
							"Error(s) parsing the selector '{0}':\n{1}",
							selectorsText,
							parser.errors.errorStream),
						GetType().FullName
					));
				}

				foreach(var selector in memberSelectors)
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

					if(visitor.IsValid)
					{
						attributeInstance.CompileTimeValidate(parameter, visitor.CurrentType, MessageSource.MessageSink);
					}
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
				parser.errors.errorStream = new StringWriter();
				selectors = parser.Parse();
				if (parser.errors.count > 0)
				{
					return;
				}
			}

			foreach (var selector in selectors)
			{
				var typeVisitor = new MemberTypeVisitor(parameter.ParameterType.GetSystemType(null, null));
				selector.Accept(typeVisitor);

				MethodBase validationMethod = ValidatorInstance.GetValidationMethod(parameter, typeVisitor.CurrentType);

				Type parameterType = parameter.ParameterType.GetSystemType(null, null);
				Type validatedType = parameter.Parent.DeclaringType.GetSystemType(null, null);

				int validationParameterIndex = validationMethod.IsStatic ? 0 : -1;

				ParameterKinds[] parameterKinds = ParameterKindDetector.GetParameterKinds(validationMethod, parameterType, validatedType);
				Debug.Assert(parameterKinds != null);

				foreach(var kind in parameterKinds)
				{
					switch(kind)
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

							// Emit the code to reach the desired member.
							var visitor = new MemberEmitterVisitor(parameter.ParameterType.GetSystemType(null, null), context, writer);
							selector.Accept(visitor);

							// Box the value if it must be converted from value-type to object.
							bool validatedParameterIsValueType = parameter.ParameterType.BelongsToClassification(TypeClassifications.ValueType);
							bool validationParameterIsValueType = validationMethod.GetParameters()[validationParameterIndex].ParameterType.IsValueType;
							if(validatedParameterIsValueType && !validationParameterIsValueType)
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
				if(validationMethod.IsStatic)
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