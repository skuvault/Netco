using System;
using System.Collections.Generic;

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

		private static readonly Dictionary< CommonTimeZone, TimeZoneInfo > _commonTimeZones = new Dictionary< CommonTimeZone, TimeZoneInfo >{
			{ CommonTimeZone.PST, TimeZoneInfo.FindSystemTimeZoneById( "Pacific Standard Time" )},
			{ CommonTimeZone.MST, TimeZoneInfo.FindSystemTimeZoneById( "Mountain Standard Time" )},
			{ CommonTimeZone.CST, TimeZoneInfo.FindSystemTimeZoneById( "Central Standard Time" )},
			{ CommonTimeZone.EST, TimeZoneInfo.FindSystemTimeZoneById( "Eastern Standard Time" )}
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