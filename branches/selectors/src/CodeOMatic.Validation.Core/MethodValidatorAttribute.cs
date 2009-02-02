using System;
using System.Collections.Generic;
using PostSharp.CodeModel;
using PostSharp.Extensibility;

namespace CodeOMatic.Validation.Core
{
	/// <summary>
	/// Base attribute for validating multiple method parameters.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public abstract class MethodValidatorAttribute : ValidatorAttribute, IMethodValidator
	{
		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on a specific method.
		/// </summary>
		/// <param name="method">The method on which the attribute is applied.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		public virtual void CompileTimeValidate(MethodDefDeclaration method, IMessageSink messages)
		{
		}

		/// <summary>
		/// Validates the parameters.
		/// </summary>
		/// <param name="target">The object on which the method is being invoked.</param>
		/// <param name="parameters">The parameter values.</param>
		public abstract void Validate(object target, IDictionary<string, object> parameters);

		/// <summary>
		/// Gets a value indicating whether this instance is persistent.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is persistent; otherwise, <c>false</c>.
		/// </value>
		public bool IsPersistent
		{
			get
			{
				return true;
			}
		}
	}
}
