using System;
using System.Globalization;
using Netco.Utils;

namespace Netco.Monads
{
	/// <summary>
	/// 	Helper routines for converting strings into Maybe
	/// </summary>
	public static class MaybeParse
	{
		/// <summary>
		/// 	Tries to parse the specified string into the enum, returning empty result
		/// 	on failure. We ignore case in this scenario.
		/// </summary>
		/// <typeparam name="TEnum">
		/// 	The type of the enum.
		/// </typeparam>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either enum or an empty result
		/// </returns>
		public static Maybe< TEnum > Enum< TEnum >( string value ) where TEnum : struct, IComparable
		{
			return Enum< TEnum >( value, true );
		}

		/// <summary>
		/// 	Tries to parse the specified string into the enum, returning empty result
		/// 	on failure
		/// </summary>
		/// <typeparam name="TEnum">
		/// 	The type of the enum.
		/// </typeparam>
		/// <param name="value">The value.</param>
		/// <param name="ignoreCase">
		/// 	if set to
		/// 	<c>true</c>
		/// 	then parsing will ignore case.
		/// </param>
		/// <returns>
		/// 	either enum or an empty result
		/// </returns>
		public static Maybe< TEnum > Enum< TEnum >( string value, bool ignoreCase ) where TEnum : struct, IComparable
		{
			if( string.IsNullOrEmpty( value ) )
				return Maybe< TEnum >.Empty;
			try
			{
				return EnumUtil.Parse< TEnum >( value, ignoreCase );
			}
			catch( Exception )
			{
				return Maybe< TEnum >.Empty;
			}
		}

		/// <summary>
		/// 	Tries to parse the specified value into Decimal, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Decimal or an empty result
		/// </returns>
		/// <seealso cref="decimal.TryParse(string,out decimal)" />
		public static Maybe< decimal > Decimal( string value )
		{
			return decimal.TryParse( value, out var result ) ? result : Maybe< decimal >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified value into decimal, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="numberStyles">
		/// 	The number styles to use.
		/// </param>
		/// <param name="formatProvider">
		/// 	The format provider to use.
		/// </param>
		/// <returns>
		/// 	either parsed decimal or an empty result
		/// </returns>
		/// <seealso cref="decimal.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out decimal)" />
		public static Maybe< decimal > Decimal( string value,
			NumberStyles numberStyles,
			IFormatProvider formatProvider )
		{
			return decimal.TryParse( value, numberStyles, formatProvider, out var result ) ? result : Maybe< decimal >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified value into decimal, using the invariant culture
		/// 	info and returning	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed decimal or an empty result
		/// </returns>
		public static Maybe< decimal > DecimalInvariant( string value )
		{
			return Decimal( value, NumberStyles.Number, CultureInfo.InvariantCulture );
		}

		/// <summary>
		/// 	Tries to parse the specified value into Int32, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Int32 or an empty result
		/// </returns>
		/// <seealso cref="System.Int32.TryParse(string,out int)" />
		public static Maybe< Int32 > Int32( string value )
		{
			return int.TryParse( value, out var result ) ? result : Maybe< Int32 >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified value into Int32, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="numberStyles">
		/// 	The number styles to use.
		/// </param>
		/// <param name="formatProvider">
		/// 	The format provider to use.
		/// </param>
		/// <returns>
		/// 	either parsed Int32 or an empty result
		/// </returns>
		/// <seealso cref="System.Int32.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out int)" />
		public static Maybe< Int32 > Int32( string value,
			NumberStyles numberStyles,
			IFormatProvider formatProvider )
		{
			return int.TryParse( value, numberStyles, formatProvider, out var result ) ? result : Maybe< Int32 >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified string value into Int32, 
		/// 	using an invariant culture and returning empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Int32 or an empty result
		/// </returns>
		/// <seealso cref="System.Int32.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out int)" />
		public static Maybe< int > Int32Invariant( string value )
		{
			return Int32( value, NumberStyles.Integer, CultureInfo.InvariantCulture );
		}

		/// <summary>
		/// 	Tries to parse the specified value into Int64, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Int64 or an empty result
		/// </returns>
		/// <seealso cref="System.Int64.TryParse(string,out long)" />
		public static Maybe< long > Int64( string value )
		{
			return long.TryParse( value, out var result ) ? result : Maybe< long >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified value into Int64, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="numberStyles">
		/// 	The number styles to use.
		/// </param>
		/// <param name="formatProvider">
		/// 	The format provider to use.
		/// </param>
		/// <returns>
		/// 	either parsed Int64 or an empty result
		/// </returns>
		/// <seealso cref="System.Int64.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out long)" />
		public static Maybe< Int64 > Int64( string value,
			NumberStyles numberStyles,
			IFormatProvider formatProvider )
		{
			return long.TryParse( value, numberStyles, formatProvider, out var result ) ? result : Maybe< long >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified string value into Int64, 
		/// 	using an invariant culture and returning empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Int64 or an empty result
		/// </returns>
		/// <seealso cref="System.Int64.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out long)" />
		public static Maybe< long > Int64Invariant( string value )
		{
			return Int64( value, NumberStyles.Integer, CultureInfo.InvariantCulture );
		}

		/// <summary>
		/// 	Tries to parse the specified value into Double, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Double or an empty result
		/// </returns>
		/// <seealso cref="System.Double.TryParse(string,out double)" />
		public static Maybe< double > Double( string value )
		{
			return double.TryParse( value, out var result ) ? result : Maybe< double >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified value into Double, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="numberStyles">
		/// 	The number styles to use.
		/// </param>
		/// <param name="formatProvider">
		/// 	The format provider to use.
		/// </param>
		/// <returns>
		/// 	either parsed Double or an empty result
		/// </returns>
		/// <seealso cref="System.Double.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out double)" />
		public static Maybe< double > Double( string value,
			NumberStyles numberStyles,
			IFormatProvider formatProvider )
		{
			return double.TryParse( value, numberStyles, formatProvider, out var result ) ? result : Maybe< double >.Empty;
		}

		/// <summary>
		/// 	Attempts to parse the specified value into Double, 
		/// 	using invariant culture and returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Double or an empty result
		/// </returns>
		/// <seealso cref="System.Double.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out double)" />
		public static Maybe< Double > DoubleInvariant( string value )
		{
			return Double( value,
				NumberStyles.Float | NumberStyles.AllowThousands,
				CultureInfo.InvariantCulture );
		}

		/// <summary>
		/// 	Tries to parse the specified value into Single, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Single or an empty result
		/// </returns>
		/// <seealso cref="System.Single.TryParse(string,out float)" />
		public static Maybe< float > Single( string value )
		{
			return float.TryParse( value, out var result ) ? result : Maybe< float >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified value into Single, returning
		/// 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="numberStyles">
		/// 	The number styles to use.
		/// </param>
		/// <param name="formatProvider">
		/// 	The format provider to use.
		/// </param>
		/// <returns>
		/// 	either parsed Single or an empty result
		/// </returns>
		/// <seealso cref="System.Single.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out float)" />
		public static Maybe< float > Single( string value,
			NumberStyles numberStyles,
			IFormatProvider formatProvider )
		{
			return float.TryParse( value, numberStyles, formatProvider, out var result ) ? result : Maybe< float >.Empty;
		}

		/// <summary>
		/// 	Tries to parse the specified value into Single, using invariant culture
		/// 	and returning 	empty result on failure.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	either parsed Single or an empty result
		/// </returns>
		/// <seealso cref="System.Single.TryParse(string,System.Globalization.NumberStyles,System.IFormatProvider,out float)" />
		public static Maybe< Single > SingleInvariant( string value )
		{
			return Single( value,
				NumberStyles.Float | NumberStyles.AllowThousands,
				CultureInfo.InvariantCulture );
		}
	}
}