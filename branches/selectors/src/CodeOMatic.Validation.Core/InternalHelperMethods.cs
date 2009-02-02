using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CodeOMatic.Validation.Core
{
	/// <summary>
	/// Defines some usefull methods for internal use only.
	/// </summary>
	public static class InternalHelperMethods
	{
		/// <summary>
		/// Deserializes an object from the specified string.
		/// </summary>
		/// <param name="serializedValue">The serialized object.</param>
		/// <returns></returns>
		public static object DeserializeFromString(string serializedValue)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			using(MemoryStream buffer = new MemoryStream(Convert.FromBase64String(serializedValue)))
			{
				return formatter.Deserialize(buffer);
			}
		}

		/// <summary>
		/// Serializes the specified object to string.
		/// </summary>
		/// <param name="value">The object to serialize.</param>
		/// <returns></returns>
		public static string SerializeToString(object value)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			using (MemoryStream buffer = new MemoryStream())
			{
				formatter.Serialize(buffer, value);
				return Convert.ToBase64String(buffer.ToArray());
			}
		}

		/// <summary>
		/// Creates a parameter collection.
		/// </summary>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static ParameterDictionary CreateParameterCollection()
		{
			return new ParameterDictionary();
		}
	}
}