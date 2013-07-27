using System;

namespace Netco.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="TimeSpan"/>.
	/// </summary>
	public static class TimeSpanExtensions
	{
		/// <summary>
		/// Converts <see cref="string"/> to <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns><see cref="TimeSpan"/> created from the supplied string.</returns>
		/// <remarks>Converts "1.5h", "30m", or "1" (default hour) to <see cref="TimeSpan"/>.</remarks>
		public static TimeSpan ToTimeSpan( this string s )
		{
			if( string.IsNullOrEmpty( s ) )
				return TimeSpan.Zero;

			if( s.EndsWith( "m" ) )
				return TimeSpan.FromMinutes( double.Parse( s.Replace( "m", string.Empty ) ) );
			else //if( TimeAllocated.EndsWith( "h" ))
				return TimeSpan.FromHours( double.Parse( s.Replace( "h", string.Empty ) ) );
		}

		/// <summary>
		/// Converts <see cref="TimeSpan"/> to string.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <returns>String representing time span.</returns>
		public static string ToStringFormat( this TimeSpan timeSpan )
		{
			return ToStringFormat( timeSpan, null );
		}

		/// <summary>
		/// Converts <see cref="TimeSpan"/> to string.
		/// </summary>
		/// <param name="timeSpan">The time span.</param>
		/// <param name="format">The <see cref="DateTime.ToString(string)"/> format.</param>
		/// <returns>Formatted <see cref="TimeSpan"/> string.</returns>
		/// <remarks>If <paramref name="format"/> is <c>null</c> or empty default formatting is used.
		/// <para>With default formatting if time span is less than an hour, total minutes are shown followed by "m" (<b>15m</b>).
		/// Otherwise total hours are shown followed by "h" (<b>1.5h</b>).</para></remarks>
		public static string ToStringFormat( this TimeSpan timeSpan, string format )
		{
			if( !string.IsNullOrEmpty( format ) )
			{
				if( timeSpan < TimeSpan.Zero )
					timeSpan = TimeSpan.Zero;

				var dt = DateTime.MinValue.Add( timeSpan );
				return dt.ToString( format );
			}
			else
			{
				if( timeSpan.TotalHours >= 1 )
					return Math.Round( timeSpan.TotalHours, 1 ) + "h";
				else
					return Math.Floor( timeSpan.TotalMinutes ) + "m";
			}
		}
	}
}