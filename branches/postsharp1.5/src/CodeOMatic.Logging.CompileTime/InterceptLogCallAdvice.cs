using System;
using System.Globalization;
using log4net;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using System.Reflection;

namespace CodeOMatic.Logging.CompileTime
{
	internal sealed class InterceptLogCallAdvice : BaseLogAdvice
	{
		private readonly MethodDefDeclaration interceptedMethod;

		public InterceptLogCallAdvice(MethodDefDeclaration interceptedMethod)
		{
			this.interceptedMethod = interceptedMethod;
		}

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
			var loggerField = GetLoggerField(context, true);
			writer.EmitInstructionField(OpCodeNumber.Ldsfld, loggerField);

			writer.EmitInstructionMethod(OpCodeNumber.Call, GetLoggerMethod(context.Method.Module));
		}

		private IMethod GetLoggerMethod(ModuleDeclaration module)
		{
			var systemMethod = interceptedMethod.GetSystemMethod(null, null, BindingOptions.Default);
			var systemParameters = systemMethod.GetParameters();

			var parameterTypes = new Type[systemParameters.Length + 1];
			for(int i = 0; i < systemParameters.Length; ++i)
			{
				parameterTypes[i] = systemParameters[i].ParameterType;
			}
			parameterTypes[systemParameters.Length] = typeof(ILog);

			var loggerMethod = Log.LoggerType.GetMethod(systemMethod.Name, BindingFlags.Public | BindingFlags.Static, null, parameterTypes, null);
			if (loggerMethod == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "There is a bug in CodeOMatic.Validation. The method '{0}' does not exist in the Logger class", interceptedMethod));
			}

			return module.FindMethod(loggerMethod, BindingOptions.Default);
		}
	}
}