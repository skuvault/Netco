using System.Collections.Generic;

namespace Netco.Extensions
{
	/// <summary>
	/// Provides helper methods for <see cref="string"/>
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Calls <see cref="string.Format( string, object )"/>
		/// </summary>
		/// <param name="string">The string.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The resulting string.</returns>
		public static string FormatWith( this string @string, params object[] args )
		{
			return string.Format( @string, args );
		}

		/// <summary>
		/// Calls <see cref="string.Join( string, IEnumerable{ string } )" />
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="separator">The separator.</param>
		/// <returns>The resulting string.</returns>
		public static string JoinWith( this IEnumerable< string > values, string separator )
		{
			return string.Join( separator, values );
		}
	}
}