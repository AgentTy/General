using System;

namespace General.Exceptions
{
	/// <summary>
	/// CUSTOM ERROR CLASS
	/// </summary>
	public class RecordExistsException : Exception
	{

		/// <summary>
		/// Create a new Err Object
		/// </summary>
		public RecordExistsException(string Message) : base(Message)
		{

		}

		/// <summary>
		/// Create a new Err Object
		/// </summary>
		public RecordExistsException(string Message, Exception Inner) : base(Message,Inner)
		{

		}

	}
}
