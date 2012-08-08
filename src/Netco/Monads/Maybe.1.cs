using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Netco.Monads
{
	/// <summary>
	/// Helper class that indicates nullable value in a good-citizenship code
	/// </summary>
	/// <typeparam name="T">underlying type</typeparam>
	[ Serializable ]
	public sealed class Maybe< T > : IEquatable< Maybe< T > >
	{
		private readonly T _value;

		private Maybe( T item, bool hasValue )
		{
			this._value = item;
			this.HasValue = hasValue;
		}

		internal Maybe( T value ) : this( value, true )
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			Contract.Requires< ArgumentNullException >( value != null, "value" );
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// Default empty instance.
		/// </summary>
		/// <remarks>Not static anymore to correctly support meta data for empty maybe.</remarks>
		public static Maybe< T > Empty
		{
			get { return new Maybe< T >( default( T ), false ); }
		}

		/// <summary>
		/// Gets the underlying value.
		/// </summary>
		/// <value>The value.</value>
		public T Value
		{
			get
			{
				Contract.Requires< InvalidOperationException >( this.HasValue, "Code should not access value when it is not available." );
				return this._value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has value.
		/// </summary>
		/// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
		public bool HasValue { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance has NO value.
		/// </summary>
		/// <value><c>true</c> if this instance has NO value; otherwise, <c>false</c>.</value>
		public bool HasNothing
		{
			get { return !this.HasValue; }
		}

		/// <summary>
		/// Retrieves value from this instance, using a 
		/// <paramref name="defaultValue"/> if it is absent.
		/// </summary>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>value</returns>
		public T GetValue( Func< T > defaultValue )
		{
			return this.HasValue ? this._value : defaultValue();
		}

		/// <summary>
		/// Retrieves value from this instance, using a 
		/// <paramref name="defaultValue"/> if it is absent.
		/// </summary>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>value</returns>
		public T GetValue( T defaultValue )
		{
			return this.HasValue ? this._value : defaultValue;
		}

		/// <summary>
		/// Retrieves value from this instance, using a <paramref name="defaultValue"/>
		/// factory, if it is absent
		/// </summary>
		/// <param name="defaultValue">The default value to provide.</param>
		/// <returns>maybe value</returns>
		public Maybe< T > GetValue( Func< Maybe< T > > defaultValue )
		{
			return this.HasValue ? this : defaultValue();
		}

		/// <summary>
		/// Retrieves value from this instance, using a <paramref name="defaultValue"/>
		/// if it is absent
		/// </summary>
		/// <param name="defaultValue">The default value to provide.</param>
		/// <returns>maybe value</returns>
		public Maybe< T > GetValue( Maybe< T > defaultValue )
		{
			return this.HasValue ? this : defaultValue;
		}

		/// <summary>
		/// Applies the specified action to the value, if it is present.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>same instance for inlining</returns>
		public Maybe< T > Apply( Action< T > action )
		{
			if( this.HasValue )
				action( this._value );

			return this;
		}

		/// <summary>
		/// Applies the specified action to the value with it's current meta data, if it is present.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>same instance for inlining</returns>
		public Maybe< T > Apply( Action< T, MetaData > action )
		{
			if( this.HasValue )
				action( this._value, this.MetaData );

			return this;
		}

		/// <summary>
		/// Executes the specified action, if the value is absent
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>same instance for inlining</returns>
		public Maybe< T > Handle( Action action )
		{
			if( !this.HasValue )
				action();

			return this;
		}

		/// <summary>
		/// Exposes the specified exception if maybe does not have value.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>actual value</returns>
		/// <exception cref="Exception">if maybe does not have value</exception>
		public T ExposeException( Func< Exception > exception )
		{
			if( !this.HasValue )
				throw exception();

			return this._value;
		}

		/// <summary>
		/// Throws the exception if maybe does not have value.
		/// </summary>
		/// <returns>actual value</returns>
		/// <exception cref="InvalidOperationException">if maybe does not have value</exception>
		public T ExposeException( string message, params object[] args )
		{
			Contract.Requires< ArgumentNullException >( message != null, "message" );
			Contract.Requires< ArgumentException >( !string.IsNullOrWhiteSpace( message ), "message is a zero-length string or contains only white space" );
			if( !this.HasValue )
				throw new InvalidOperationException( string.Format( message, args ) );

			return this._value;
		}

		/// <summary>
		/// Combines this optional with the pipeline function
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="combinator">The combinator (pipeline funcion).</param>
		/// <returns>optional result</returns>
		public Maybe< TTarget > Combine< TTarget >( Func< T, Maybe< TTarget > > combinator )
		{
			return this.HasValue ? combinator( this._value ) : Maybe< TTarget >.Empty;
		}

		/// <summary>
		/// Combines this optional with the pipeline function, providing MetaData
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="combinator">The combinator (pipeline funcion).</param>
		/// <returns>optional result</returns>
		public Maybe< TTarget > Combine< TTarget >( Func< T, MetaData, Maybe< TTarget > > combinator )
		{
			return this.HasValue ? combinator( this._value, this.MetaData ) : Maybe< TTarget >.Empty;
		}

		/// <summary>
		/// Converts this instance to <see cref="Maybe{T}"/>, 
		/// while applying <paramref name="converter"/> if there is a value.
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		public Maybe< TTarget > Convert< TTarget >( Func< T, TTarget > converter )
		{
			return this.HasValue ? converter( this._value ) : Maybe< TTarget >.Empty;
		}

		/// <summary>
		/// Converts this instance to <see cref="Maybe{T}"/>, 
		/// while applying <paramref name="converter"/> if there is a value.
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		public Maybe< TTarget > Convert< TTarget >( Func< T, MetaData, TTarget > converter )
		{
			return this.HasValue ? converter( this._value, this.MetaData ) : Maybe< TTarget >.Empty;
		}

		/// <summary>
		/// Retrieves converted value, using a 
		/// <paramref name="defaultValue"/> if it is absent.
		/// </summary>
		/// <typeparam name="TTarget">type of the conversion target</typeparam>
		/// <param name="converter">The converter.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>value</returns>
		public TTarget Convert< TTarget >( Func< T, TTarget > converter, Func< TTarget > defaultValue )
		{
			return this.HasValue ? converter( this._value ) : defaultValue();
		}

		/// <summary>
		/// Retrieves converted value, using a 
		/// <paramref name="defaultValue"/> if it is absent.
		/// </summary>
		/// <typeparam name="TTarget">type of the conversion target</typeparam>
		/// <param name="converter">The converter.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>value</returns>
		public TTarget Convert< TTarget >( Func< T, MetaData, TTarget > converter, Func< TTarget > defaultValue )
		{
			return this.HasValue ? converter( this._value, this.MetaData ) : defaultValue();
		}

		/// <summary>
		/// Retrieves converted value, using a 
		/// <paramref name="defaultValue"/> if it is absent.
		/// </summary>
		/// <typeparam name="TTarget">type of the conversion target</typeparam>
		/// <param name="converter">The converter.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>value</returns>
		public TTarget Convert< TTarget >( Func< T, TTarget > converter, TTarget defaultValue )
		{
			return this.HasValue ? converter( this._value ) : defaultValue;
		}

		/// <summary>
		/// Retrieves converted value, using a 
		/// <paramref name="defaultValue"/> if it is absent.
		/// </summary>
		/// <typeparam name="TTarget">type of the conversion target</typeparam>
		/// <param name="converter">The converter.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>value</returns>
		public TTarget Convert< TTarget >( Func< T, MetaData, TTarget > converter, TTarget defaultValue )
		{
			return this.HasValue ? converter( this._value, this.MetaData ) : defaultValue;
		}

		/// <summary>
		/// Determines whether the specified <see cref="Maybe{T}"/> is equal to the current <see cref="Maybe{T}"/>.
		/// </summary>
		/// <param name="maybe">The <see cref="Maybe"/> to compare with.</param>
		/// <returns>true if the objects are equal</returns>
		public bool Equals( Maybe< T > maybe )
		{
			if( ReferenceEquals( null, maybe ) )
				return false;
			if( ReferenceEquals( this, maybe ) )
				return true;

			if( this.HasValue != maybe.HasValue )
				return false;
			if( !this.HasValue )
				return true;
			return this._value.Equals( maybe._value );
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <paramref name="obj"/> parameter is null.
		/// </exception>
		public override bool Equals( object obj )
		{
			if( ReferenceEquals( null, obj ) )
				return false;
			if( ReferenceEquals( this, obj ) )
				return true;

			var maybe = obj as Maybe< T >;
			if( maybe == null )
				return false;
			return this.Equals( maybe );
		}

		/// <summary>
		/// Serves as a hash function for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="Maybe{T}"/>.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				// ReSharper disable CompareNonConstrainedGenericWithNull
				return ( ( this._value != null ? this._value.GetHashCode() : 0 ) * 397 ) ^ this.HasValue.GetHashCode();
				// ReSharper restore CompareNonConstrainedGenericWithNull
			}
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==( Maybe< T > left, Maybe< T > right )
		{
			return Equals( left, right );
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=( Maybe< T > left, Maybe< T > right )
		{
			return !Equals( left, right );
		}

		/// <summary>
		/// Performs an implicit conversion from <typeparamref name="T"/> to <see cref="Maybe{T}"/>.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator Maybe< T >( T item )
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			Contract.Requires< ArgumentNullException >( item != null, "item" );
			// ReSharper restore CompareNonConstrainedGenericWithNull

			return new Maybe< T >( item );
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="Maybe{T}"/> to <typeparamref name="T"/>.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator T( Maybe< T > item )
		{
			Contract.Requires< ArgumentNullException >( item != null, "item" );
			Contract.Requires< ArgumentException >( item.HasValue, "item must have value." );

			return item.Value;
		}

		/// <summary>
		/// Converts maybe into result, using the specified error as the failure
		/// descriptor
		/// </summary>
		/// <typeparam name="TError">The type of the failure.</typeparam>
		/// <param name="error">The error.</param>
		/// <returns>result describing current maybe</returns>
		public Result< T, TError > Join< TError >( TError error )
		{
			if( this.HasValue )
				return this._value;
			return error;
		}

		/// <summary>
		/// Converts maybe into result, using the specified error as the failure
		/// descriptor
		/// </summary>
		/// <returns>result describing current maybe</returns>
		public Result< T > JoinMessage( string error )
		{
			if( this.HasValue )
				return this._value;
			return error;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if( this.HasValue )
				return "<" + this._value + ">";

			return "<Empty>";
		}

		/// <summary>
		/// Gets the meta data.
		/// </summary>
		public MetaData MetaData
		{
			get { return this._metaData ?? ( this._metaData = new MetaData() ); }
		}

		private MetaData _metaData;

		/// <summary>
		/// Adds the meta data regardless if there's value or not.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>Current instance for pipe calls.</returns>
		public Maybe< T > AddMeta( object key, object value )
		{
			this.MetaData[ key ] = value;
			return this;
		}

		/// <summary>
		/// Applies the meta data if there's value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>This object for pipe calls.</returns>
		public Maybe< T > ApplyMeta( object key, object value )
		{
			if( key == null )
				throw new ArgumentNullException( "key" );
			if( this.HasValue )
				this.MetaData[ key ] = value;
			return this;
		}

		/// <summary>
		/// Gets the meta data.
		/// </summary>
		/// <typeparam name="TMeta">The type of the meta.</typeparam>
		/// <param name="metaKey">The meta key.</param>
		/// <returns>Meta data casted to the specified type.</returns>
		/// <exception cref="KeyNotFoundException">Specified <paramref name="metaKey"/> key was not found</exception>
		public TMeta GetMeta< TMeta >( object metaKey )
		{
			if( metaKey == null )
				throw new ArgumentNullException( "metaKey" );
			if( this._metaData != null )
				return this._metaData.GetValue< TMeta >( metaKey );
			throw new KeyNotFoundException( string.Format( "'{0}' key was not found", metaKey ) );
		}

		/// <summary>
		/// Attempts to get meta data.
		/// </summary>
		/// <typeparam name="TMeta">The type of the meta.</typeparam>
		/// <param name="metaKey">The meta key.</param>
		/// <returns>Maybe with the specified meta data if meta data was found and successfully casted to the specified type.
		/// Otherwise empty Maybe is returned.</returns>
		public Maybe< TMeta > TryGetMeta< TMeta >( object metaKey )
		{
			if( metaKey == null )
				throw new ArgumentNullException( "metaKey" );
			if( this._metaData != null )
				return this._metaData.TryGetValue< TMeta >( metaKey );

			return Maybe< TMeta >.Empty;
		}
	}
}