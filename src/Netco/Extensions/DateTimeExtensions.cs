using System;
using System.Collections.Generic;
using System.Globalization;

namespace Netco.Extensions
{
	/// <summary>
	/// Provides helper methods for <see cref="DateTime"/>.
	/// </summary>
	public static class DateTimeExtensions
	{
		static DateTimeExtensions()
		{
			LocalTimeZone = TimeZoneInfo.Local;
		}

		/// <summary>
		/// Parses string to <see cref="DateTime"/> keeping it in UTC format.
		/// </summary>
		/// <param name="dateTimeString">The date time string.</param>
		/// <returns><see cref="DateTime"/> in UTC.</returns>
		public static DateTime ParseToUtc( this string dateTimeString )
		{
			return DateTime.Parse( dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal );
		}

		/// <summary>
		/// Subtracts from <see cref="DateTime"/> to minimum value to avoid going beyoind <see cref="DateTime.MinValue"/>.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <param name="value">The value.</param>
		/// <returns>New <see cref="DateTime"/> value.</returns>
		public static DateTime SubtractToMinValue( this DateTime dateTime, TimeSpan value )
		{
			return dateTime.Ticks > value.Ticks ? dateTime.Subtract( value ) : DateTime.MinValue;
		}

		#region TimeZone
		/// <summary>
		/// Gets or sets the local time zone used by <see cref="ToPresetLocal"/>.
		/// </summary>
		/// <value>The local time zone.</value>
		public static TimeZoneInfo LocalTimeZone{ get; set; }

		/// <summary>
		/// Converts <see cref="DateTime"/> to local time zone.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>Local <see cref="DateTime"/>.</returns>
		/// <remarks>Relies on <see cref="LocalTimeZone"/> for conversion.</remarks>
		public static DateTime ToPresetLocal( this DateTime dateTime )
		{
			return TimeZoneInfo.ConvertTime( dateTime, LocalTimeZone );
		}

		/// <summary>
		/// Sets the local time zone.
		/// </summary>
		/// <param name="timeZone">The time zone.</param>
		public static void SetLocalTimeZone( CommonTimeZone timeZone )
		{
			LocalTimeZone = _commonTimeZones[ timeZone ];
		}

		private static readonly Dictionary< CommonTimeZone, TimeZoneInfo > _commonTimeZones = new Dictionary< CommonTimeZone, TimeZoneInfo >
		{
			{ CommonTimeZone.PST, TimeZoneInfo.FindSystemTimeZoneById( "Pacific Standard Time" ) },
			{ CommonTimeZone.MST, TimeZoneInfo.FindSystemTimeZoneById( "Mountain Standard Time" ) },
			{ CommonTimeZone.CST, TimeZoneInfo.FindSystemTimeZoneById( "Central Standard Time" ) },
			{ CommonTimeZone.EST, TimeZoneInfo.FindSystemTimeZoneById( "Eastern Standard Time" ) }
		};

		/// <summary>
		/// Commonly supported time zones.
		/// </summary>
		/// <seealso href="http://www.timeanddate.com/library/abbreviations/timezones/na/"/>
		public enum CommonTimeZone
		{
			/// <summary>
			/// Pacific Standard Time
			/// </summary>
			/// <remarks>UTC - 8 hours, EST - 3 hours</remarks>
			PST,

			/// <summary>
			/// Mountain Standard Time
			/// </summary>
			/// <remarks>UTC - 7 hours, EST - 2 hours, PST + 1 hour</remarks>
			MST,

			/// <summary>
			/// Central Standard Time
			/// </summary>
			/// <remarks>UTC - 6 hours, EST - 1 hour, PST + 2 hours</remarks>
			CST,

			/// <summary>
			/// Eastern Standard Time
			/// </summary>
			/// <remarks>UTC - 5 hours, PST + 3 hours</remarks>
			EST
		}
		#endregion
	}
}