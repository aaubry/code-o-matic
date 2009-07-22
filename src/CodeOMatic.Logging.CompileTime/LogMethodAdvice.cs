using System;
using PostSharp.CodeModel;
using PostSharp.CodeModel.TypeSignatures;
using PostSharp.CodeWeaver;
using System.Reflection;
using PostSharp.Collections;
using log4net;
using System.Diagnostics;

namespace CodeOMatic.Logging.CompileTime
{
	internal sealed class LogMethodAdvice : BaseLogAdvice
	{
		public override int Priority
		{
			get
			{
				return 1;
			}
		}

		public override bool RequiresWeave(WeavingContext context)
		{
			return true;
		}

		protected override void Weave(WeavingContext context, InstructionWriter writer)
		{
			switch(context.JoinPoint.JoinPointKind)
			{
				case JoinPointKinds.BeforeMethodBody:
					WeaveBeforeMethodBody(context, writer);
					break;
				
				case JoinPointKinds.AfterMethodBodySuccess:
					WeaveAfterMethodBodySuccess(context, writer);
					break;
	
				case JoinPointKinds.AfterMethodBodyException:
					WeaveAfterMethodBodyException(context, writer);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static readonly Type loggerAttributeHelperType = typeof(Log).GetNestedType("LogAttributeHelper");

		private static LogAttribute GetLogAttribute(WeavingContext context)
		{
			var logAttributeType = context.Method.Module.FindType(typeof(LogAttribute), BindingOptions.Default);
			foreach (var attribute in context.Method.CustomAttributes)
			{
				if (logAttributeType.Equals(attribute.Constructor.DeclaringType))
				{
					return (LogAttribute)attribute.ConstructRuntimeObject();
				}
			}
			throw new InvalidOperationException("This should never happen!");
		}

		private static void WeaveBeforeMethodBody(WeavingContext context, InstructionWriter writer)
		{
			var attribute = GetLogAttribute(context);
			if (attribute.EntryLevel != LogLevel.None)
			{
				string logLevel = attribute.EntryLevel.ToString();
				EmitLevelTest(context, writer, logLevel);

				// Method name
				writer.EmitInstructionString(OpCodeNumber.Ldstr, GetMetodFullName(context));

				// Message
				if (attribute.EntryMessage != null)
				{
					writer.EmitInstructionString(OpCodeNumber.Ldstr, attribute.EntryMessage);
				}
				else
				{
					writer.EmitInstruction(OpCodeNumber.Ldnull);
				}

				// Argument names
				var parameters = context.Method.Parameters;
				writer.EmitInstructionInt32(OpCodeNumber.Ldc_I4, parameters.Count);

				var module = context.Method.Module;
				var stringType = module.FindType(typeof(string), BindingOptions.Default);
				writer.EmitInstructionType(OpCodeNumber.Newarr, stringType);

				for (int i = 0; i < parameters.Count; ++i)
				{
					writer.EmitInstruction(OpCodeNumber.Dup);
					writer.EmitInstructionInt32(OpCodeNumber.Ldc_I4, i);
					writer.EmitInstructionString(OpCodeNumber.Ldstr, parameters[i].Name);
					writer.EmitInstruction(OpCodeNumber.Stelem_Ref);
				}

				// Arguments
				writer.EmitInstructionInt32(OpCodeNumber.Ldc_I4, parameters.Count);

				var objectType = module.FindType(typeof(object), BindingOptions.Default);
				writer.EmitInstructionType(OpCodeNumber.Newarr, objectType);

				for (int i = 0; i < parameters.Count; ++i)
				{
					writer.EmitInstruction(OpCodeNumber.Dup);
					writer.EmitInstructionInt32(OpCodeNumber.Ldc_I4, i);

					writer.EmitInstructionInt32(OpCodeNumber.Ldarg, i);

					var parameter = parameters[i];
					if (parameter.ParameterType.BelongsToClassification(TypeClassifications.ValueType))
					{
						writer.EmitInstructionType(OpCodeNumber.Box, parameter.ParameterType);
					}

					writer.EmitInstruction(OpCodeNumber.Stelem_Ref);
				}

				// Logger
				writer.EmitInstructionField(OpCodeNumber.Ldsfld, GetLoggerField(context, true));

				// Call
				var systemEnterLevel = loggerAttributeHelperType.GetMethod("Enter" + logLevel, BindingFlags.Static | BindingFlags.Public);
				Debug.Assert(systemEnterLevel != null);
				var enterLevel = module.FindMethod(systemEnterLevel, BindingOptions.Default);
				writer.EmitInstructionMethod(OpCodeNumber.Call, enterLevel);
			}
		}

		private static void WeaveAfterMethodBodySuccess(WeavingContext context, InstructionWriter writer)
		{
			var attribute = GetLogAttribute(context);
			if (attribute.ExitLevel != LogLevel.None)
			{
				string logLevel = attribute.ExitLevel.ToString();
				EmitLevelTest(context, writer, logLevel);

				// Method name
				writer.EmitInstructionString(OpCodeNumber.Ldstr, GetMetodFullName(context));

				// Message
				if (attribute.EntryMessage != null)
				{
					writer.EmitInstructionString(OpCodeNumber.Ldstr, attribute.ExitMessage);
				}
				else
				{
					writer.EmitInstruction(OpCodeNumber.Ldnull);
				}

				// Has return parameter
				var returnType = context.Method.ReturnParameter.ParameterType;

				bool hasReturnType = false;
				if (returnType.BelongsToClassification(TypeClassifications.Intrinsic))
				{
					var intrinsicType = returnType as IntrinsicTypeSignature;
					hasReturnType = intrinsicType == null || intrinsicType.IntrinsicType != IntrinsicType.Void;
				}
				writer.EmitInstructionInt32(OpCodeNumber.Ldc_I4, hasReturnType ? 1 : 0);

				// Return parameter
				if (hasReturnType)
				{
					writer.EmitInstructionLocalVariable(OpCodeNumber.Ldloc, context.ReturnValueVariable);
					if (returnType.BelongsToClassification(TypeClassifications.ValueType))
					{
						writer.EmitInstructionType(OpCodeNumber.Box, returnType);
					}
				}
				else
				{
					writer.EmitInstruction(OpCodeNumber.Ldnull);
				}

				// Logger
				writer.EmitInstructionField(OpCodeNumber.Ldsfld, GetLoggerField(context, true));

				// Call
				var systemLeaveLevel = loggerAttributeHelperType.GetMethod("Leave" + logLevel, BindingFlags.Static | BindingFlags.Public);
				Debug.Assert(systemLeaveLevel != null);
				var leaveLevel = context.Method.Module.FindMethod(systemLeaveLevel, BindingOptions.Default);
				writer.EmitInstructionMethod(OpCodeNumber.Call, leaveLevel);
			}
		}

		private static void WeaveAfterMethodBodyException(WeavingContext context, InstructionWriter writer)
		{
			var attribute = GetLogAttribute(context);
			if (attribute.ExceptionLevel != LogLevel.None)
			{
				string logLevel = attribute.ExceptionLevel.ToString();
				var branchTarget = EmitLevelTest(context, writer, logLevel);

				// Exception
				writer.EmitInstruction(OpCodeNumber.Dup);

				// Method name
				writer.EmitInstructionString(OpCodeNumber.Ldstr, GetMetodFullName(context));

				// Message
				if (attribute.EntryMessage != null)
				{
					writer.EmitInstructionString(OpCodeNumber.Ldstr, attribute.ExceptionMessage);
				}
				else
				{
					writer.EmitInstruction(OpCodeNumber.Ldnull);
				}

				// Logger
				writer.EmitInstructionField(OpCodeNumber.Ldsfld, GetLoggerField(context, true));

				// Call
				var systemExceptionLevel = loggerAttributeHelperType.GetMethod("Exception" + logLevel, BindingFlags.Static | BindingFlags.Public);
				Debug.Assert(systemExceptionLevel != null);
				var exceptionLevel = context.Method.Module.FindMethod(systemExceptionLevel, BindingOptions.Default);
				writer.EmitInstructionMethod(OpCodeNumber.Call, exceptionLevel);

				// throw
				writer.DetachInstructionSequence();
				writer.AttachInstructionSequence(branchTarget);
				writer.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);
				writer.EmitInstruction(OpCodeNumber.Rethrow);
			}
		}

		private static InstructionSequence EmitLevelTest(WeavingContext context, InstructionWriter writer, string logLevel)
		{
			var loggerField = GetLoggerField(context, true);
			writer.EmitInstructionField(OpCodeNumber.Ldsfld, loggerField);

			// If level is enabled
			var systemIsLevelEnabled = typeof(ILog).GetProperty("Is" + logLevel + "Enabled").GetGetMethod();
			var isLevelEnabled = context.Method.Module.FindMethod(systemIsLevelEnabled, BindingOptions.Default);
			writer.EmitInstructionMethod(OpCodeNumber.Callvirt, isLevelEnabled);

			var branchTarget = context.Method.MethodBody.CreateInstructionSequence();
			writer.CurrentInstructionSequence.ParentInstructionBlock.AddInstructionSequence(branchTarget, NodePosition.After, writer.CurrentInstructionSequence);

			writer.EmitBranchingInstruction(OpCodeNumber.Brfalse, branchTarget);
			return branchTarget;
		}

		private static string GetMetodFullName(WeavingContext context)
		{
			return context.Method.DeclaringType.Name + "." + context.Method.Name;
		}
	}
}