using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;

namespace Netco.Extensions
{
	/// <summary>
	/// Provides helper methods for <see cref="Dictionary{TKey,TValue}"/>
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Gets the value from dictionary, or <paramref name="defaultValue"/> if there's no value for specified <paramref name="key"/>.</returns>
		public static TValue GetValue< TKey, TValue >( this Dictionary< TKey, TValue > dictionary, TKey key, TValue defaultValue = default( TValue ) )
		{
			if( dictionary == null )
				return defaultValue;

			TValue value;
			return dictionary.TryGetValue( key, out value ) ? value : defaultValue;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValueFactory">The default value factory.</param>
		/// <returns>Gets the value from dictionary, or value produced by <paramref name="defaultValueFactory"/> if there's no value for specified <paramref name="key"/>.</returns>	
		public static TValue GetValue< TKey, TValue >( this Dictionary< TKey, TValue > dictionary, TKey key, Func< TValue > defaultValueFactory )
		{
			Condition.Requires( defaultValueFactory, "defaultValueFactory" ).IsNotNull();
			if( dictionary == null )
				return defaultValueFactory();

			TValue value;
			return dictionary.TryGetValue( key, out value ) ? value : defaultValueFactory();
		}
	}
}