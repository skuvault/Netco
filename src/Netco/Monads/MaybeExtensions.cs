using System;
using System.Collections.Generic;

namespace Netco.Monads
{
	///<summary>
	/// Provides extension methods for Maybe monad.
	///</summary>
	public static class MaybeExtensions
	{
		/// <summary>
		/// Selects <typeparamref name="TSource"/> using <paramref name="sourceToMaybeResult"/>.
		/// </summary>
		/// <typeparam name="TSource">Source type.</typeparam>
		/// <typeparam name="TResult">Expected result type to wrap in <see cref="Maybe"/>.</typeparam>
		/// <param name="source">Source object to select.</param>
		/// <param name="sourceToMaybeResult">Function to select <paramref name="source"/> in order to get <typeparamref name="TResult"/>.</param>
		/// <returns><see cref="Maybe"/> class, possibly with results of the <paramref name="sourceToMaybeResult"/>.</returns>
		/// <seealso href="http://jystic.com/2009/09/08/whats-with-nullreferenceexceptions-anyway/"/>
		public static Maybe< TResult > SelectMany< TSource, TResult >( this Maybe< TSource > source, Func< TSource, Maybe< TResult > > sourceToMaybeResult )
		{
			return source.Bind( sourceToMaybeResult );
		}

		/// <summary>
		/// Selects <typeparamref name="TA"/> using <paramref name="aToMaybeB"/> and <paramref name="abToMaybeC"/>.
		/// </summary>
		/// <typeparam name="TA">The type of the source.</typeparam>
		/// <typeparam name="TB">The type of the intermidiate.</typeparam>
		/// <typeparam name="TC">The type of the result.</typeparam>
		/// <param name="maybeA">The source.</param>
		/// <param name="aToMaybeB">The second value selector.</param>
		/// <param name="abToMaybeC">The result selector.</param>
		/// <returns><see cref="Maybe"/> class, possibly with results of the <paramref name="abToMaybeC"/>.</returns>
		/// <remarks>Required to use LINQ for selection.</remarks>
		/// <example>
		/// var packagedSalad = from apple in PickApple()
		/// from orange in PickOrange()
		/// from fruitSalad in MakeFruitSalad(apple, orange)
		/// select PackageForSale(fruitSalad);
		/// </example>
		public static Maybe< TC > SelectMany< TA, TB, TC >( this Maybe< TA > maybeA, Func< TA, Maybe< TB > > aToMaybeB, Func< TA, TB, Maybe< TC > > abToMaybeC )
		{
			return maybeA.Bind( a => ( aToMaybeB( a ) ?? Maybe< TB >.Empty ).Bind( b => abToMaybeC( a, b ) ) );
		}

		/// <summary>
		/// Gets the value associated  with the specified key.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <returns><see cref="Maybe{T}"/> with the result, or empty <see cref="Maybe{T}"/> if there's no object in the dictionary
		/// with the specified key.</returns>
		public static Maybe< TValue > TryGetValue< TKey, TValue >( this IDictionary< TKey, TValue > dictionary, TKey key )
		{
			TValue value;
			return dictionary.TryGetValue( key, out value ) ? value : Maybe< TValue >.Empty;
		}
	}
}