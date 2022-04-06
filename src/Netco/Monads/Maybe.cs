using System;

namespace Netco.Monads
{
	/// <summary>
	/// Helper routines for <see cref="Maybe{T}"/>
	/// </summary>
	public static class Maybe
	{
		//public static T? ToNullable<T>(this Maybe<T> maybe) where T : struct
		//{
		//    return maybe.HasValue
		//        ? maybe.Value
		//        : new T?();
		//}

		//public static Maybe<T> ToMaybe<T>(this T? nullable) where T : struct
		//{
		//    return nullable.HasValue
		//        ? nullable.Value
		//        : Maybe<T>.Empty;
		//}

		/// <summary>
		/// Creates new <see cref="Maybe{T}"/> from the provided value
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="item">The item.</param>
		/// <returns><see cref="Maybe{T}"/> that matches the provided value</returns>
		/// <exception cref="ArgumentNullException">if argument is a null reference</exception>
		public static Maybe< TSource > From< TSource >( TSource item )
		{
			if( item == null )
				throw new ArgumentNullException( nameof(item) );

			return new Maybe< TSource >( item );
		}

		/// <summary>
		/// Optional empty boolean
		/// </summary>
		public static readonly Maybe< bool > Bool = Maybe< bool >.Empty;

		/// <summary>
		/// Optional empty string
		/// </summary>
		public static readonly Maybe< string > String = Maybe< string >.Empty;
	}
}