using System;

namespace Netco.Extensions
{
	/// <summary>
	/// Provides helper methods for <see cref="Exception"/>
	/// </summary>
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Determines if <paramref name="exception"/> or any of it's inner exceptions satisfy the provided predicate.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns><c>true</c> if <paramref name="predicate"/> evaluated to <c>true</c> to <paramref name="exception"/> or any of it's inner exceptions.
		/// <c>false</c> if no match was found.</returns>
		public static bool AnyException( this Exception exception, Func< Exception, bool > predicate )
		{
			var currentException = exception;
			var iterationCount = 0;
			while( currentException != null && iterationCount < 5 )
			{
				if( predicate( currentException ) )
					return true;
				currentException = currentException.InnerException;
				iterationCount++;
			}
			return false;
		}
	}
}