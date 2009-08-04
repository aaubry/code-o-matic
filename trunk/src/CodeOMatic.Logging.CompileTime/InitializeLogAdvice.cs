using System;
using PostSharp.CodeModel.Helpers;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using log4net;
using System.Diagnostics;

namespace CodeOMatic.Logging.CompileTime
{
	internal sealed class InitializeLogAdvice : BaseLogAdvice
	{
		public override int Priority
		{
			get
			{
				return 0;
			}
		}

		public override bool RequiresWeave(WeavingContext context)
		{
			return GetLoggerField(context, false) != null;
		}

		protected override void Weave(WeavingContext context, InstructionWriter writer)
		{
			ITypeSignature declaringType = context.Method.DeclaringType;
			if (declaringType.IsGenericDefinition)
			{
				var canonicalType = GenericHelper.GetTypeCanonicalGenericInstance(declaringType.GetTypeDefinition(BindingOptions.RequireGenericDefinition));
				var genericContext = canonicalType.GetGenericContext(GenericContextOptions.ResolveGenericParameterDefinitions);
				declaringType = canonicalType.MapGenericArguments(genericContext);
			}
			writer.EmitInstructionType(OpCodeNumber.Ldtoken, declaringType);

			var typeGetType = typeof(Type).GetMethod("GetTypeFromHandle");
			writer.EmitInstructionMethod(OpCodeNumber.Call, context.Method.Module.FindMethod(typeGetType, BindingOptions.Default));

			var getLogger = context.Method.Module.FindMethod(typeof(LogManager).GetMethod("GetLogger", new[] { typeof(Type) }), BindingOptions.Default);
			writer.EmitInstructionMethod(OpCodeNumber.Call, getLogger);

			var loggerField = GetLoggerField(context, false);
			writer.EmitInstructionField(OpCodeNumber.Stsfld, loggerField);
		}
	}
}