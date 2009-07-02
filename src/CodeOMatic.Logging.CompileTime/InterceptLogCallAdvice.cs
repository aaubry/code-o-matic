using System;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using System.Reflection;
using PostSharp.Collections;

namespace CodeOMatic.Logging.CompileTime
{
	internal class InterceptLogCallAdvice : IAdvice
	{
		internal const string loggerFieldName = "logger_qrE7YQg07D";

		private readonly MethodDefDeclaration interceptedMethod;

		public InterceptLogCallAdvice(MethodDefDeclaration interceptedMethod)
		{
			this.interceptedMethod = interceptedMethod;
		}

		#region IAdvice Members
		int IAdvice.Priority
		{
			get
			{
				return 1;
			}
		}

		bool IAdvice.RequiresWeave(WeavingContext context)
		{
			return true;
		}

		void IAdvice.Weave(WeavingContext context, InstructionBlock block)
		{
			InstructionSequence entrySequence = context.Method.MethodBody.CreateInstructionSequence();
			block.AddInstructionSequence(entrySequence, NodePosition.Before, null);

			InstructionWriter writer = context.InstructionWriter;
			var loggerMethod = GetLoggerMethod(context.Method.DeclaringType, writer);

			writer.AttachInstructionSequence(entrySequence);
			writer.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);

			writer.EmitInstructionMethod(OpCodeNumber.Call, loggerMethod);

			writer.DetachInstructionSequence();
		}
		#endregion

		private static string GetMethodStubName(string baseName)
		{
			return string.Concat(loggerFieldName, "_", baseName);
		}

		private MethodDefDeclaration GetLoggerMethod(TypeDefDeclaration type, InstructionWriter writer)
		{
			while(true)
			{
				foreach(var currentMethod in type.Methods)
				{
					if(currentMethod.Name == GetMethodStubName(interceptedMethod.Name))
					{
						return currentMethod;
					}
				}

				var module = type.Module;

				FieldDefDeclaration field = new FieldDefDeclaration();
				field.Name = loggerFieldName;
				field.FieldType = module.FindType(Log.LoggerType, BindingOptions.Default);
				field.Attributes = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;
				type.Fields.Add(field);

				foreach (var method in Log.LoggerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
				{
					MethodDefDeclaration injectedMethod = new MethodDefDeclaration();
					injectedMethod.Name = GetMethodStubName(method.Name);
					injectedMethod.Attributes = MethodAttributes.Private | MethodAttributes.Static;
					type.Methods.Add(injectedMethod);

					injectedMethod.ReturnParameter = new ParameterDeclaration();
					injectedMethod.ReturnParameter.ParameterType = module.FindType(method.ReturnType, BindingOptions.Default);
					injectedMethod.ReturnParameter.Attributes = ParameterAttributes.Retval;

					var methodParameters = method.GetParameters();
					foreach (var parameter in methodParameters)
					{
						ParameterDeclaration injectedParameter = new ParameterDeclaration();
						injectedParameter.Name = parameter.Name;
						injectedParameter.ParameterType = module.FindType(parameter.ParameterType, BindingOptions.Default);
						injectedParameter.Ordinal = parameter.Position;
						injectedMethod.Parameters.Add(injectedParameter);
					}

					injectedMethod.MethodBody = new MethodBodyDeclaration();
					InstructionSequence entrySequence = injectedMethod.MethodBody.CreateInstructionSequence();

					injectedMethod.MethodBody.RootInstructionBlock = injectedMethod.MethodBody.CreateInstructionBlock();

					injectedMethod.MethodBody.RootInstructionBlock.AddInstructionSequence(entrySequence, NodePosition.Before, null);

					writer.AttachInstructionSequence(entrySequence);
					writer.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);

					writer.EmitInstructionField(OpCodeNumber.Ldsfld, field);
					for(int i = 0; i < methodParameters.Length; ++i)
					{
						writer.EmitInstructionInt32(OpCodeNumber.Ldarg, i);
					}

					writer.EmitInstructionMethod(OpCodeNumber.Callvirt, module.FindMethod(method, BindingOptions.Default));

					writer.EmitInstruction(OpCodeNumber.Ret);

					writer.DetachInstructionSequence();
				}
			}
		}
	}
}