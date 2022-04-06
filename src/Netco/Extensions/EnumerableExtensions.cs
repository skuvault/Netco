using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netco.Monads;

namespace Netco.Extensions
{
	/// <summary>
	/// Provides extensions for <see cref="IEnumerable{T}" />
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Applies the specified action to the target <paramref name="enumerable" />.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="action">The action to execute against every item.</param>
		/// <returns>enumerator</returns>
		/// <exception cref="ArgumentNullException">when one of the values is null</exception>
		public static IEnumerable< T > Apply< T >( this IEnumerable< T > enumerable, Action< T > action )
		{
			if( enumerable == null )
				throw new ArgumentNullException( "enumerable" );
			if( action == null )
				throw new ArgumentNullException( "action" );

			foreach( var t in enumerable )
			{
				action( t );
				yield return t;
			}
		}

		/// <summary>
		/// Applies the specified action to the target <paramref name="enumerable" />.
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in <paramref name="enumerable" /></typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="action">
		/// The action to execute against every item; second
		/// parameter represents the index.
		/// </param>
		/// <returns>enumerator</returns>
		/// <exception cref="ArgumentNullException">when one of the values is null</exception>
		public static IEnumerable< TSource > Apply< TSource >( this IEnumerable< TSource > enumerable, Action< TSource, int > action )
		{
			if( enumerable == null )
				throw new ArgumentNullException( nameof(enumerable) );

			if( action == null )
				throw new ArgumentNullException( nameof(action) );

			var i = 0;
			foreach( var t in enumerable )
			{
				action( t, i );
				yield return t;
				i += 1;
			}
		}

		/// <summary>
		/// Clumps items into same size lots.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source list of items.</param>
		/// <param name="size">The maximum size of the clumps to make.</param>
		/// <returns>A list of list of items, where each list of items is no bigger than the size given.</returns>
		public static IEnumerable< IEnumerable< T > > Clump< T >( this IEnumerable< T > source, int size )
		{
			if( source is null )
				throw new ArgumentNullException( nameof(source) );

			if( size < 1 )
				throw new ArgumentOutOfRangeException( nameof(size), $"{nameof(size)} must be at least 1" );

			return ClumpIterator( source, size );
		}

		/// <summary>
		/// Returns <em>True</em> as soon as the first member of <paramref name="enumerable" />
		/// matches <paramref name="predicate" />
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in <paramref name="enumerable" /></typeparam>
		/// <param name="enumerable">The enumerable</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>
		/// true if the <paramref name="enumerable" /> contains any elements
		/// matching <paramref name="predicate" />
		/// </returns>
		public static bool Exists< TSource >( this IEnumerable< TSource > enumerable, Predicate< TSource > predicate )
		{
			if( enumerable == null )
				throw new ArgumentNullException( nameof(enumerable) );

			if( predicate == null )
				throw new ArgumentNullException( nameof(predicate) );

			return enumerable.Any( t => predicate( t ) );
		}

		/// <summary>
		/// Checks if the provided enumerable has anything
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in <paramref name="enumerable" /></typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <returns>true if the sequence contains any elements</returns>
		public static bool Exists< TSource >( this IEnumerable< TSource > enumerable )
		{
			if( enumerable == null )
				throw new ArgumentNullException( nameof(enumerable) );

			return enumerable.Any();
		}

		/// <summary>
		/// Retrieves first value from the <paramref name="sequence" />
		/// </summary>
		/// <typeparam name="TSource">The type of the source sequence.</typeparam>
		/// <param name="sequence">The source.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>first value</returns>
		public static Maybe< TSource > FirstOrEmpty< TSource >( this IEnumerable< TSource > sequence, Func< TSource, bool > predicate )
		{
			if( sequence is null )
				throw new ArgumentNullException( nameof(sequence) );

			if( predicate is null )
				throw new ArgumentNullException( nameof(predicate) );

			foreach( var source in sequence )
				if( predicate( source ) )
					return source;

			return Maybe< TSource >.Empty;
		}

		/// <summary>
		/// Retrieves first value from the <paramref name="sequence" />
		/// </summary>
		/// <typeparam name="TSource">The type of the source sequence.</typeparam>
		/// <param name="sequence">The source.</param>
		/// <returns>first value or empty result, if it is not found</returns>
		public static Maybe< TSource > FirstOrEmpty< TSource >( this IEnumerable< TSource > sequence )
		{
			if( sequence == null )
				throw new ArgumentNullException( nameof(sequence) );
			foreach( var source in sequence )
				return source;

			return Maybe< TSource >.Empty;
		}

		/// <summary>
		/// Performs the specified <see cref="Action{T}" /> against every element of <see cref="IEnumerable{T}" />
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable">Enumerable to extend</param>
		/// <param name="action">Action to perform; second parameter represents the index</param>
		/// <exception cref="ArgumentNullException">When any parameter is null</exception>
		public static void ForEach< T >( this IEnumerable< T > enumerable, Action< T, int > action )
		{
			if( enumerable == null )
				throw new ArgumentNullException( "enumerable" );
			if( action == null )
				throw new ArgumentNullException( "action" );

			var i = 0;
			foreach( var t in enumerable )
			{
				action( t, i );
				i += 1;
			}
		}

		/// <summary>
		/// Performs the specified <see cref="Action{T}" /> on each element in the source sequence.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the sequence</typeparam>
		/// <param name="enumerable">The sequence of elements</param>
		/// <param name="action">The action to execute on each element</param>
		public static void ForEach< T >( this IEnumerable< T > enumerable, Action< T > action )
		{
			if( enumerable == null )
				throw new ArgumentNullException( "enumerable" );
			if( action == null )
				throw new ArgumentNullException( "action" );
			foreach( var element in enumerable )
				action( element );
		}

		/// <summary>
		/// Gets the enumerable hash code computed based on the items in enumerable.
		/// </summary>
		/// <param name="enumerable">The enumerable.</param>
		/// <returns>Hash code computed based on the items in enumerable</returns>
		public static int GetEnumerableHashCode( this IEnumerable enumerable )
		{
			var hashCode = 0;
			if( enumerable == null )
				return hashCode;

			unchecked
			{
				foreach( var value in enumerable )
					hashCode = ( hashCode * 397 ) ^ ( value?.GetHashCode() ?? 0 );
			}

			return hashCode;
		}

		/// <summary>
		/// Gets the enumerable hash code.
		/// </summary>
		/// <param name="enumerable">The enumerable.</param>
		/// <returns>Hash code computed based on the items in enumerable</returns>
		public static int GetEnumerableHashCode( params object[] enumerable )
		{
			return enumerable.GetEnumerableHashCode();
		}

		/// <summary>
		/// Gets the specific page.
		/// </summary>
		/// <typeparam name="T">Type of the elements in <paramref name="enumerable" /></typeparam>
		/// <param name="enumerable">The source.</param>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns>Page with elements.</returns>
		public static IEnumerable< T > GetPage< T >( this IEnumerable< T > enumerable, int pageNumber, int pageSize )
		{
			return enumerable.Skip( pageNumber * pageSize ).Take( pageSize );
		}

		/// <summary>
		/// Checks whether two enumerables are equal, even if they're null.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		/// <returns><c>true</c> if they're equal, <c>false</c> otherwise.</returns>
		public static bool ListEqual< TSource >( this IEnumerable< TSource > first, IEnumerable< TSource > second )
		{
			if( first == second )
				return true;
			if( first == null || second == null )
				return false;
			return first.SequenceEqual( second );
		}

		/// <summary>
		/// Selects the values from a sequence of optionals.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="sequence">The sequence.</param>
		/// <returns>enumerable that contains values</returns>
		public static IEnumerable< TValue > SelectValues< TValue >( this IEnumerable< Maybe< TValue > > sequence )
		{
			return sequence.Where( s => s.HasValue ).Select( s => s.Value );
		}

		/// <summary>
		///     <para>Performs lazy splitting of the provided collection into collections of <paramref name="sliceLength" />.</para>
		///     <para>Each collection will have total <em>weight</em> equal or less than <paramref name="maxSliceWeight" /></para>
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="source">The source collection to slice.</param>
		/// <param name="sliceLength">Length of the slice.</param>
		/// <param name="weightDelegate">Function to calculate <em>weight</em> of each item in the collection</param>
		/// <param name="maxSliceWeight">The max item weight.</param>
		/// <returns>enumerator over the results</returns>
		public static IEnumerable< TItem[] > Slice< TItem >( this IEnumerable< TItem > source, int sliceLength, Func< TItem, int > weightDelegate, int maxSliceWeight )
		{
			if( source == null )
				throw new ArgumentNullException( nameof(source) );

			if( weightDelegate == null )
				throw new ArgumentNullException( nameof(weightDelegate) );

			if( sliceLength <= 0 )
				throw new ArgumentOutOfRangeException( nameof(sliceLength), "value must be greater than 0" );

			if( maxSliceWeight <= 0 )
				throw new ArgumentOutOfRangeException( nameof(sliceLength), "value must be greater than 0" );

			var list = new List< TItem >( sliceLength );
			var accumulatedWeight = 0;
			foreach( var item in source )
			{
				var currentWeight = weightDelegate( item );
				if( currentWeight > maxSliceWeight )
					throw new InvalidOperationException( "Impossible to slice this collection" );

				var weightOverload = currentWeight + accumulatedWeight > maxSliceWeight;
				if( sliceLength == list.Count || weightOverload )
				{
					accumulatedWeight = 0;
					yield return list.ToArray();
					list.Clear();
				}

				list.Add( item );
				accumulatedWeight += currentWeight;
			}

			if( list.Count > 0 )
				yield return list.ToArray();
		}

		/// <summary>
		/// Performs lazy splitting of the provided collection into collections of <paramref name="sliceLength" />
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="sliceLength">Maximum length of the slice.</param>
		/// <returns>lazy enumerator of the collection of arrays</returns>
		public static IEnumerable< TItem[] > Slice< TItem >( this IEnumerable< TItem > source, int sliceLength )
		{
			if( source == null )
				throw new ArgumentNullException( nameof(source) );

			if( sliceLength <= 0 )
				throw new ArgumentOutOfRangeException( nameof(sliceLength), "value must be greater than 0" );

			var list = new List< TItem >( sliceLength );
			foreach( var item in source )
			{
				list.Add( item );
				if( sliceLength == list.Count )
				{
					yield return list.ToArray();
					list.Clear();
				}
			}

			if( list.Count > 0 )
				yield return list.ToArray();
		}

		/// <summary>
		/// Converts the enumerable to <see cref="HashSet{T}" />
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="selector">The selector.</param>
		/// <returns>hashset instance</returns>
		public static HashSet< TKey > ToHashSet< TKey, TItem >( this IEnumerable< TItem > enumerable, Func< TItem, TKey > selector )
		{
			if( enumerable == null )
				throw new ArgumentNullException( nameof(enumerable) );

			if( selector == null )
				throw new ArgumentNullException( nameof(selector) );

			return new HashSet< TKey >( enumerable.Select( selector ) );
		}

		/// <summary>
		/// Enumerates and returns a mapping that associated the items with their respective
		/// indices (positions) within the enumeration.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>a dictionary</returns>
		/// <remarks>
		///     <para>
		///     Typical usage is <c>coll.ToIndex()["foo"]</c> that returns
		///     the position of the item <c>"foo"</c> in the initial collection.
		///     </para>
		///     <para>
		///     if multiple similar entries are present in the original collection,
		///     index of the first entry is recorded.
		///     </para>
		/// </remarks>
		public static IDictionary< TSource, int > ToIndexDictionary< TSource >( this IEnumerable< TSource > source )
		{
			if( source == null )
				throw new ArgumentNullException( "source" );
			var dic = new Dictionary< TSource, int >( source.Count() );

			var index = 0;
			foreach( var x in source )
			{
				if( !dic.ContainsKey( x ) )
					dic.Add( x, index );
				index += 1;
			}

			return dic;
		}

		/// <summary>
		/// Applies the integral indexer to the sequence in a lazy manner
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The sequence.</param>
		/// <returns>indexed sequence</returns>
		/// <exception cref="ArgumentNullException"> if <paramref name="source" /> is null</exception>
		public static IEnumerable< Indexer< TSource > > ToIndexed< TSource >( this IEnumerable< TSource > source )
		{
			if( source == null )
				throw new ArgumentNullException( "source" );
			var index = 0;

			foreach( var item in source )
			{
				yield return new Indexer< TSource >( index, item );
				index += 1;
			}
		}

		/// <summary>
		/// Converts collection of collections to jagged array
		/// </summary>
		/// <typeparam name="T">type of the items in collection</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>jagged array</returns>
		public static T[][] ToJaggedArray< T >( this IEnumerable< IEnumerable< T > > collection )
		{
			if( collection == null )
				throw new ArgumentNullException( nameof(collection) );
			return collection.Select( i => i.ToArray() ).ToArray();
		}

		private static IEnumerable< IEnumerable< T > > ClumpIterator< T >( IEnumerable< T > source, int size )
		{
			var items = new T[ size ];
			var count = 0;
			foreach( var item in source )
			{
				items[ count ] = item;
				count++;

				if( count == size )
				{
					yield return items;
					items = new T[ size ];
					count = 0;
				}
			}

			if( count > 0 )
			{
				if( count == size )
				{
					yield return items;
				} else
				{
					var tempItems = new T[ count ];
					Array.Copy( items, tempItems, count );
					yield return tempItems;
				}
			}
		}

		#region Async
		#endregion
	}

	/// <summary>
	/// Indexing wrapper that contains value and its integral position.
	/// </summary>
	/// <typeparam name="TSource">type of the underlying item</typeparam>
	public struct Indexer< TSource >
	{
		internal Indexer( int index, TSource value )
		{
			this.Index = index;
			this.IsFirst = index == 0;
			this.Value = value;
		}

		/// <summary>
		/// Gets the integral position of the item.
		/// </summary>
		/// <value>The integral position of the item.</value>
		public int Index{ get; }

		/// <summary>
		/// Gets a value indicating whether this instance is first.
		/// </summary>
		/// <value><c>true</c> if this instance is first; otherwise, <c>false</c>.</value>
		public bool IsFirst{ get; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public TSource Value{ get; }
	}
}