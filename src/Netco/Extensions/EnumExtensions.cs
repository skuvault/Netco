using System;
using System.Collections.Generic;
using System.Linq;

namespace Netco.Extensions
{
	/// <summary>
	/// Provides helper methods for <see cref="Enum"/>
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Converts string value enum of type T.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>Enum value.</returns>
		public static T ToEnum< T >( this string value ) where T : struct
		{
			value = value.Replace( " ", "" );
			T result;
			if( !Enum.TryParse( value, true, out result ) || !Enum.IsDefined( typeof( T ), result ) )
				throw new ArgumentException( "EnumType does not has a constant equal to value", nameof( value ) );

			return result;
		}

		/// <summary>
		/// Converts string value enum of type T, or uses <paramref name="defaultValue"/> if conversion cannot take place.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Enum value, or <paramref name="defaultValue"/> if conversion failed.</returns>
		public static T ToEnum< T >( this string value, T defaultValue ) where T : struct
		{
			if( string.IsNullOrWhiteSpace( value ) )
				return defaultValue;

			value = value.Replace( " ", "" );
			T result;
			if( !Enum.TryParse( value, true, out result ) || !Enum.IsDefined( typeof( T ), result ) )
				return defaultValue;

			return result;
		}

		/// <summary>
		/// Converts all possible enum values to list.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <returns>List of possible values.</returns>
		public static IList< T > ConvertToList< T >()
		{
			return Enum.GetValues( typeof( T ) ).Cast< T >().ToList();
		}
	}
}