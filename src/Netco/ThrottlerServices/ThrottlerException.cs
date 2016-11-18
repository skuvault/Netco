using System;

namespace Netco.ThrottlerServices
{
	/// <summary>
	/// </summary>
	public class ThrottlerException : Exception
	{
		/// <summary>
		/// </summary>
		public ThrottlerException()
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		public ThrottlerException(string message) : base(message)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ThrottlerException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}