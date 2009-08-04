using System;
using System.Reflection;
using log4net;
using PostSharp.CodeModel;
using PostSharp.CodeModel.Helpers;
using PostSharp.CodeWeaver;
using PostSharp.Collections;
using System.Diagnostics;

namespace CodeOMatic.Logging.CompileTime
{
	internal abstract class BaseLogAdvice : IAdvice
	{
		internal const string loggerFieldName = "logger_qrE7YQg07D";

		protected static IField GetLoggerField(WeavingContext context, bool create)
		{
			var field = GetNonGenericLoggerField(context, create);
			if (field != null && context.Method.DeclaringType.IsGenericDefinition)
			{
				return GenericHelper.GetFieldCanonicalGenericInstance(field);
			}
			else
			{
				return field;
			}
		}

		private static FieldDefDeclaration GetNonGenericLoggerField(WeavingContext context, bool create)
		{
			var type = context.Method.DeclaringType;
			foreach(var currentField in type.Fields)
			{
				if(currentField.Name == loggerFieldName)
				{
					return currentField;
				}
			}

			if(create)
			{
				FieldDefDeclaration field = new FieldDefDeclaration();
				field.Name = loggerFieldName;
				field.FieldType = type.Module.FindType(typeof(ILog), BindingOptions.Default);
				field.Attributes = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;
				type.Fields.Add(field);

				return field;
			}
			else
			{
				return null;
			}
		}

		#region IAdvice Members
		public abstract int Priority
		{
			get;
		}

		public abstract bool RequiresWeave(WeavingContext context);

		public void Weave(WeavingContext context, InstructionBlock block)
		{
			InstructionSequence entrySequence = context.Method.MethodBody.CreateInstructionSequence();
			block.AddInstructionSequence(entrySequence, NodePosition.Before, null);

			InstructionWriter writer = context.InstructionWriter;

			writer.AttachInstructionSequence(entrySequence);
			writer.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);

			Weave(context, writer);

			writer.DetachInstructionSequence();
		}
		#endregion

		protected abstract void Weave(WeavingContext context, InstructionWriter writer);
	}
}