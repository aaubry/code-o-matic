using System;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using PostSharp.Collections;
using System.Diagnostics;

namespace CodeOMatic.Logging.CompileTime
{
	internal class InitializeLogAdvice : IAdvice
	{
		#region IAdvice Members
		public int Priority
		{
			get
			{
				return 0;
			}
		}

		public bool RequiresWeave(WeavingContext context)
		{
			return FindLoggerField(context) != null;
		}

		public void Weave(WeavingContext context, InstructionBlock block)
		{
			InstructionSequence entrySequence = context.Method.MethodBody.CreateInstructionSequence();
			block.AddInstructionSequence(entrySequence, NodePosition.Before, null);

			InstructionWriter writer = context.InstructionWriter;
			
			writer.AttachInstructionSequence(entrySequence);
			writer.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);

			writer.EmitInstructionType(OpCodeNumber.Ldtoken, context.Method.DeclaringType);

			var typeGetType = typeof(Type).GetMethod("GetTypeFromHandle");
			writer.EmitInstructionMethod(OpCodeNumber.Call, context.Method.Module.FindMethod(typeGetType, BindingOptions.Default));

			var logType = context.Method.Module.FindMethod(Log.LoggerType.GetConstructor(new[] { typeof(Type) }), BindingOptions.Default);
			writer.EmitInstructionMethod(OpCodeNumber.Newobj, logType);

			var loggerField = FindLoggerField(context);
			writer.EmitInstructionField(OpCodeNumber.Stsfld, loggerField);

			writer.DetachInstructionSequence();
		}
		#endregion

		private static FieldDefDeclaration FindLoggerField(WeavingContext context)
		{
			foreach (var field in context.Method.DeclaringType.Fields)
			{
				if (field.Name == InterceptLogCallAdvice.loggerFieldName)
				{
					return field;
				}
			}
			return null;
		}
	}
}