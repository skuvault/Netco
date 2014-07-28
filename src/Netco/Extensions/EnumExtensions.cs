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
		public static T ToEnum< T >( this string value )
		{
			value = value.Replace( " ", "" );
			return ( T )Enum.Parse( typeof( T ), value, true );
		}

		/// <summary>
		/// Converts string value enum of type T, or uses <paramref name="defaultValue"/> if conversion cannot take place.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Enum value, or <paramref name="defaultValue"/> if conversion failed.</returns>
		public static T ToEnum< T >( this string value, T defaultValue )
		{
			if( string.IsNullOrWhiteSpace( value ) )
				return defaultValue;

			value = value.Replace( " ", "" );
			try
			{
				return ( T )Enum.Parse( typeof( T ), value, true );
			}
			catch
			{
				return defaultValue;
			}
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