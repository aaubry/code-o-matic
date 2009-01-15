using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace CodeOMatic.Validation.Core
{
	/// <summary>
	/// Collection of parameters.
	/// </summary>
	[Serializable]
	public class ParameterDictionary : Dictionary<string, object>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterDictionary"/> class.
		/// </summary>
		[DebuggerStepThrough]
		internal ParameterDictionary()
		{	
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterDictionary"/> class.
		/// </summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"/> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param>
		protected ParameterDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{	
		}

		/// <summary>
		/// Adds a parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		[DebuggerStepThrough]
		public new void Add(string name, object value)
		{
			base.Add(name, value);
		}
	}
}
