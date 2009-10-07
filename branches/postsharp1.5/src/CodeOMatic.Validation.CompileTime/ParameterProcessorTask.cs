using System;
using System.Collections.Generic;
using PostSharp.CodeModel;
using PostSharp.CodeWeaver;
using PostSharp.Extensibility;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation.CompileTime
{
	/// <summary>
	/// 
	/// </summary>
	[CLSCompliant(false)]
	public class ParameterProcessorTask : Task, IAdviceProvider
	{
		#region IAdviceProvider Members
		/// <summary>
		/// When implemented, adds advices to a <see cref="T:PostSharp.CodeWeaver.Weaver"/>, typically
		/// using the <see cref="M:PostSharp.CodeWeaver.Weaver.AddMethodLevelAdvice(PostSharp.CodeWeaver.IAdvice,System.Collections.Generic.IEnumerable{PostSharp.CodeModel.MethodDefDeclaration},PostSharp.CodeWeaver.JoinPointKinds,System.Collections.Generic.IEnumerable{PostSharp.CodeModel.MetadataDeclaration})"/> and
		/// <see cref="M:PostSharp.CodeWeaver.Weaver.AddTypeLevelAdvice(PostSharp.CodeWeaver.IAdvice,PostSharp.CodeWeaver.JoinPointKinds,System.Collections.Generic.IEnumerable{PostSharp.CodeModel.TypeDefDeclaration})"/> methods.
		/// </summary>
		/// <param name="codeWeaver">The weaver to which advices should be added.</param>
		public void ProvideAdvices(Weaver codeWeaver)
		{
			foreach (MethodDefDeclaration method in Enumerate(Project.Module.GetDeclarationEnumerator(TokenType.MethodDef)))
			{
				int parameterIndex = 0;
				foreach (var parameter in method.Parameters)
				{
					int parameterIndexCopy = parameterIndex;
					ParameterDeclaration parameterCopy = parameter;
					AddAdvices(
						codeWeaver,
						method,
						parameter.CustomAttributes,
						typeof(IParameterValidator),
						attribute => new ParameterProcessorAdvice(attribute, parameterCopy, parameterIndexCopy),
						false
					);
					++parameterIndex;
				}

				AddAdvices(
					codeWeaver,
					method,
					method.CustomAttributes,
					typeof(MethodValidatorAttribute),
					attribute => new MethodProcessorAdvice(attribute),
					true
				);
			}

			foreach (PropertyDeclaration property in Enumerate(Project.Module.GetDeclarationEnumerator(TokenType.Property)))
			{
				if (property.CanWrite)
				{
					MethodDefDeclaration method = property.Members.GetBySemantic(MethodSemantics.Setter).Method;

					AddAdvices(
						codeWeaver,
						method,
						property.CustomAttributes,
						typeof(IParameterValidator),
						attribute => new ParameterProcessorAdvice(attribute, method.Parameters[0], 0),
						false
					);
				}
			}
		}
		#endregion

		private delegate IAdvice CreateAdvice(CustomAttributeDeclaration attribute);

		private static void AddAdvices(Weaver codeWeaver, MethodDefDeclaration method, IEnumerable<CustomAttributeDeclaration> customAttributes, Type validatorAttributeType, CreateAdvice createAdvice, bool needsParameterCollection)
		{
			bool parameterCollectionGenerated = false;

			foreach (CustomAttributeDeclaration attribute in customAttributes)
			{
				Type attributeType = attribute.Constructor.DeclaringType.GetSystemType(null, null);
				if (validatorAttributeType.IsAssignableFrom(attributeType))
				{
					if(needsParameterCollection && !parameterCollectionGenerated)
					{
						codeWeaver.AddMethodLevelAdvice(
							new BuildParameterCollectionAdvice((MethodDefDeclaration)attribute.Parent),
							new[] { method },
							JoinPointKinds.BeforeMethodBody,
							null
						);
						parameterCollectionGenerated = true;
					}

					IAdvice advice = createAdvice(attribute);
					codeWeaver.AddMethodLevelAdvice(
						advice,
						new[] { method },
						JoinPointKinds.BeforeMethodBody,
						null
					);
					codeWeaver.AddTypeLevelAdvice(
						advice,
						JoinPointKinds.BeforeStaticConstructor,
						new[] { method.DeclaringType }
					);
				}
			}
		}

		private static IEnumerable<T> Enumerate<T>(IEnumerator<T> enumerator)
		{
			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}
		}
	}
}