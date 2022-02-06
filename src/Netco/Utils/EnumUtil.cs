using System;

namespace Netco.Utils
{
	/// <summary>
	/// Enum helper class from xLim
	/// </summary>
	public static class EnumUtil
	{
		/// <summary>
		/// Parses the specified string into the <typeparamref name="TEnum"/>, ignoring the case
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>Parsed enum</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="value"/> is null</exception>
		public static TEnum Parse< TEnum >( string value ) where TEnum : struct, IComparable
		{
			if( value is null )
			{
				throw new ArgumentNullException( nameof(value) );
			}
			return Parse< TEnum >( value, true );
		}

		/// <summary>
		/// Parses the specified string into the <typeparamref name="TEnum"/>, ignoring the case
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
		/// <returns>Parsed enum</returns>
		/// <exception cref="ArgumentNullException">If <paramref name="value"/> is null</exception>
		public static TEnum Parse< TEnum >( string value, bool ignoreCase ) where TEnum : struct, IComparable
		{
			if( value is null )
			{
				throw new ArgumentNullException( nameof(value) );
			}

			var dict = ignoreCase ? EnumUtil< TEnum >.IgnoreCaseDict : EnumUtil< TEnum >.CaseDict;

			if( !dict.TryGetValue( value, out var @enum ) )
				throw new ArgumentException( $"Can't find enum for '{value}'" );
			return @enum;
		}

		/// <summary>
		/// Unwraps the enum by creating a string usable for identifiers and resource lookups.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="enumItem">The enum item.</param>
		/// <returns>a string usable for identifiers and resource lookups</returns>
		public static string ToIdentifier< TEnum >( TEnum enumItem )
			where TEnum : struct, IComparable
		{
			return EnumUtil< TEnum >.EnumPrefix + enumItem;
		}

		/// <summary>
		/// Gets the values associated with the specified enum.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <returns>array instance of the enum values</returns>
		public static TEnum[] GetValues< TEnum >() where TEnum : struct, IComparable
		{
			return EnumUtil< TEnum >.Values;
		}

		/// <summary>
		/// Gets the values associated with the specified enum, 
		/// with the exception of the default value
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <returns>array instance of the enum values</returns>
		public static TEnum[] GetValuesWithoutDefault< TEnum >() where TEnum : struct, IComparable
		{
			return EnumUtil< TEnum >.ValuesWithoutDefault;
		}
	}
}