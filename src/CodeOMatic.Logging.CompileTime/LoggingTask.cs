using System;
using System.Collections.Generic;
using PostSharp.CodeModel;
using PostSharp.CodeWeaver;
using PostSharp.Extensibility;
using System.Reflection;

namespace CodeOMatic.Logging.CompileTime
{
	/// <summary>
	/// Provides the advices that are needed to implement the logging framework.
	/// </summary>
	[CLSCompliant(false)]
	public class LoggingTask : Task, IAdviceProvider
	{
		#region IAdviceProvider Members
		/// <summary>
		/// Provides the advices.
		/// </summary>
		/// <param name="codeWeaver">The code weaver.</param>
		public void ProvideAdvices(Weaver codeWeaver)
		{
			var logType = Project.Module.FindType(typeof(Log), BindingOptions.Default);

			foreach(var method in logType.GetTypeDefinition().Methods)
			{
				if ((method.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
				{
					codeWeaver.AddMethodLevelAdvice(
						new InterceptLogCallAdvice(method),
						Cast<MetadataDeclaration, MethodDefDeclaration>(Enumerate(Project.Module.GetDeclarationEnumerator(TokenType.MethodDef))),
						JoinPointKinds.InsteadOfCall,
						new MetadataDeclaration[] {method}
					);
				}
			}

			codeWeaver.AddMethodLevelAdvice(new LogMethodAdvice(), GetLoggedMethods(), JoinPointKinds.BeforeMethodBody | JoinPointKinds.AfterMethodBodySuccess | JoinPointKinds.AfterMethodBodyException, null);

			codeWeaver.AddTypeLevelAdvice(new InitializeLogAdvice(), JoinPointKinds.BeforeStaticConstructor, GetLogTypes(Project.Module.Types));
		}

		private static IEnumerable<TypeDefDeclaration> GetLogTypes(IEnumerable<TypeDefDeclaration> types)
		{
			if (types != null)
			{
				foreach(var declaration in types)
				{
					if(declaration.BelongsToClassification(TypeClassifications.Class) ||
						declaration.BelongsToClassification(TypeClassifications.Struct))
					{
						yield return declaration;

						foreach(var innerType in GetLogTypes(declaration.Types))
						{
							yield return innerType;
						}
					}
				}
			}
		}
		#endregion

		private IEnumerable<MethodDefDeclaration> GetLoggedMethods()
		{
			var logAttributeType = Project.Module.FindType(typeof(LogAttribute), BindingOptions.Default);
			foreach (var type in Project.Module.Types)
			{
				foreach(var method in type.Methods)
				{
					foreach(var attribute in method.CustomAttributes)
					{
						if (logAttributeType.Equals(attribute.Constructor.DeclaringType))
						{
							yield return method;
						}
					}
				}
			}
		}
        
		private static IEnumerable<U> Cast<T, U>(IEnumerable<T> enumerable)
		{
			foreach(var item in enumerable)
			{
				yield return (U)(object)item;
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