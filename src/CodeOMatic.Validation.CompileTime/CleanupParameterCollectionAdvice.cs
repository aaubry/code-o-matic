using System;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;

namespace CodeOMatic.Validation.CompileTime
{
	[CLSCompliant(false)]
	public class CleanupParameterCollectionAdvice : IAdvice
	{
		#region IAdvice Members
		/// <summary>
		/// Gets the priority.
		/// </summary>
		/// <value>The priority.</value>
		public int Priority
		{
			get
			{
				return -1;
			}
		}

		/// <summary>
		/// Requireses the weave.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public bool RequiresWeave(WeavingContext context)
		{
			return context.JoinPoint.JoinPointKind == JoinPointKinds.BeforeMethodBody;
		}

		/// <summary>
		/// Weaves the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="block">The block.</param>
		public void Weave(WeavingContext context, InstructionBlock block)
		{
		}
		#endregion
	}
}