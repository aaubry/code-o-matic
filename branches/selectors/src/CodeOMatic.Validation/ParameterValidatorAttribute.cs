using System;
using System.Reflection;
using PostSharp.Extensibility;
using PostSharp.CodeModel;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation
{
	/// <summary>
	/// Base class for validating a single parameter.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public abstract class ParameterValidatorAttribute : ValidatorAttribute, IParameterValidator
	{
		/// <summary>
		/// Gets the <see cref="ParameterInfo"/> corresponding to the specified <see cref="ParameterDeclaration"/>.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		protected static ParameterInfo GetSystemParameter(ParameterDeclaration parameter)
		{
			ParameterInfo[] systemParameters = parameter.DeclaringMethod.GetSystemMethod(null, null, BindingOptions.Default).GetParameters();
			foreach(var systemParameter in systemParameters)
			{
				if(systemParameter.Name == parameter.Name)
				{
					return systemParameter;
				}
			}
			throw new ArgumentException("The specified parameter is invalid.");
		}

		/// <summary>
		/// Method called at compile-time to validate the application of this
		/// custom attribute on a specific parameter.
		/// </summary>
		/// <param name="parameter">The parameter on which the attribute is applied.</param>
		/// <param name="memberType">The type that will be validated by the attribute.</param>
		/// <param name="messages">A <see cref="IMessageSink"/> where to write error messages.</param>
		/// <remarks>
		/// This method should use <paramref name="messages"/> to report any error encountered
		/// instead of throwing an exception.
		/// </remarks>
		[CLSCompliant(false)]
		public virtual void CompileTimeValidate(ParameterDeclaration parameter, Type memberType, IMessageSink messages)
		{
		}

		/// <summary>
		/// Validates the specified value.
		/// </summary>
		/// <param name="target">The object on which the method is being called.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public abstract void Validate(object target, object value, string parameterName);

		/// <summary>
		/// Gets the validation method.
		/// </summary>
		/// <param name="parameter">The parameter to be validated.</param>
		/// <param name="memberType">The type that will be validated by the attribute.</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public virtual MethodBase GetValidationMethod(ParameterDeclaration parameter, Type memberType)
		{
			return GetType().GetMethod("Validate");
		}

		private string selectors;

		/// <summary>
		/// Gets a string that specifies on what elements of the parameter the attribute should be aplied.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// The selectors have the following syntax:
		/// <pre>
		/// Selectors =	Selector { selectorseparator Selector }.
		/// Selector = [Member] [Iteration] { ',' Member [Iteration] }.
		/// Iteration = '*'.
		/// Member = valid_C#_identifier.
		/// </pre>
		/// Each selector specifies the path to the members on which the attribute should applied.
		/// Members are C# identifiers that represent the name of a proerty on the current object.
		/// </remarks>
		/// <example>
		/// 	<code>
		/// public void SendEmail(
		/// [NotNull(Selector = ", Name, Email")]			// Checks that:
		/// Address from,									//  - the parameter itself is not null
		/// //  - the Name and Email properties are not null
		/// [NotNull(Selector = ", *, *.Name, *.Email")]	// Checks that:
		/// IEnumerable&lt;Address&gt; to					//  - the parameter itself is not null
		/// //  - each element is not null
		/// //  - the Name and Email properties of each element are not null
		/// )
		/// {
		/// // ...
		/// }
		/// </code>
		/// </example>
		public string Selectors
		{
			get
			{
				return selectors;
			}
			set
			{
				selectors = value;
			}
		}

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