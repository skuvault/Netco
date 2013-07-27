using System;
using CuttingEdge.Conditions;

namespace Netco.Monads
{
	/// <summary>
	/// Helper class that allows to pass out method call results without using exceptions
	/// </summary>
	/// <typeparam name="T">type of the associated data</typeparam>
	public sealed class Result< T > : IEquatable< Result< T > >
	{
		private readonly T _value;
		private readonly string _error;

		private Result( bool isSuccess, T value, string error )
		{
			this.IsSuccess = isSuccess;
			this._value = value;
			this._error = error;
		}

		/// <summary>
		/// Error message associated with this failure
		/// </summary>
		[ Obsolete( "Use Error instead" ) ]
		public string ErrorMessage
		{
			get { return this._error; }
		}

		/// <summary>  Creates failure result </summary>
		/// <param name="errorFormatString">format string for the error message</param>
		/// <param name="args">The arguments.</param>
		/// <returns>result that is a failure</returns>
		/// <exception cref="ArgumentNullException">if format string is null</exception>
		public static Result< T > CreateError( string errorFormatString, params object[] args )
		{
			Condition.Requires( errorFormatString, "errorFormatString" ).IsNotNull();

			return CreateError( string.Format( errorFormatString, args ) );
		}

		/// <summary>
		/// Creates the success result.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>result encapsulating the success value</returns>
		/// <exception cref="ArgumentNullException">if value is a null reference type</exception>
		public static Result< T > CreateSuccess( T value )
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if( value == null )
				throw new ArgumentNullException( "value" );
			// ReSharper restore CompareNonConstrainedGenericWithNull

			return new Result< T >( true, value, default( string ) );
		}

		/// <summary>
		/// Converts value of this instance
		/// using the provided <paramref name="converter"/>
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <returns>Converted result</returns>
		/// <exception cref="ArgumentNullException"> if <paramref name="converter"/> is null</exception>
		public Result< TTarget > Convert< TTarget >( Func< T, TTarget > converter )
		{
			Condition.Requires( converter, "converter" ).IsNotNull();
			if( !this.IsSuccess )
				return Result< TTarget >.CreateError( this._error );

			return converter( this._value );
		}

		/// <summary>
		/// Creates the error result.
		/// </summary>
		/// <param name="error">The error.</param>
		/// <returns>result encapsulating the error value</returns>
		/// <exception cref="ArgumentNullException">if error is null</exception>
		public static Result< T > CreateError( string error )
		{
			Condition.Requires( error, "error" ).IsNotNull();

			return new Result< T >( false, default( T ), error );
		}

		/// <summary>
		/// Performs an implicit conversion from <typeparamref name="T"/> to <see cref="Result{T}"/>.
		/// </summary>
		/// <param name="value">The item.</param>
		/// <returns>The result of the conversion.</returns>
		/// <exception cref="ArgumentNullException">if <paramref name="value"/> is a reference type that is null</exception>
		public static implicit operator Result< T >( T value )
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if( value == null )
				throw new ArgumentNullException( "value" );
			// ReSharper restore CompareNonConstrainedGenericWithNull
			return new Result< T >( true, value, null );
		}

		/// <summary>
		/// Combines this <see cref="Result{T}"/> with the result returned
		/// by <paramref name="converter"/>.
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <returns>Combined result.</returns>
		public Result< TTarget > Combine< TTarget >( Func< T, Result< TTarget > > converter )
		{
			if( !this.IsSuccess )
				return Result< TTarget >.CreateError( this._error );

			return converter( this._value );
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
			if( obj.GetType() != typeof( Result< T > ) )
				return false;
			return this.Equals( ( Result< T > )obj );
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var result = this.IsSuccess.GetHashCode();
				result = ( result * 397 ) ^ this._value.GetHashCode();
				result = ( result * 397 ) ^ ( this._error != null ? this._error.GetHashCode() : 0 );
				return result;
			}
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public bool Equals( Result< T > other )
		{
			if( ReferenceEquals( null, other ) )
				return false;
			if( ReferenceEquals( this, other ) )
				return true;
			return other.IsSuccess.Equals( this.IsSuccess ) && Equals( other._value, this._value ) && Equals( other._error, this._error );
		}

		/// <summary>
		/// Applies the specified <paramref name="action"/>
		/// to this <see cref="Result{T}"/>, if it has value.
		/// </summary>
		/// <param name="action">The action to apply.</param>
		/// <returns>returns same instance for inlining</returns>
		/// <exception cref="ArgumentNullException">if <paramref name="action"/> is null</exception>
		public Result< T > Apply( Action< T > action )
		{
			Condition.Requires( action, "action" ).IsNotNull();
			if( this.IsSuccess )
				action( this._value );

			return this;
		}

		/// <summary>
		/// Handles the specified handler.
		/// </summary>
		/// <param name="handler">The handler.</param>
		/// <returns>same instance for the inlining</returns>
		public Result< T > Handle( Action< string > handler )
		{
			Condition.Requires( handler, "handler" ).IsNotNull();

			if( !this.IsSuccess )
				handler( this._error );

			return this;
		}

		/// <summary>
		/// Gets a value indicating whether this result is valid.
		/// </summary>
		/// <value><c>true</c> if this result is valid; otherwise, <c>false</c>.</value>
		public bool IsSuccess { get; private set; }

		/// <summary>
		/// item associated with this result
		/// </summary>
		public T Value
		{
			get
			{
				Condition.WithExceptionOnFailure< InvalidOperationException >().Requires( this.IsSuccess, "IsSuccess" ).IsTrue( "Code should not access value when the result has failed." );
				return this._value;
			}
		}

		/// <summary>
		/// Error message associated with this failure
		/// </summary>
		public string Error
		{
			get
			{
				Condition.WithExceptionOnFailure< InvalidOperationException >().Requires( this.IsSuccess, "IsSuccess" ).IsFalse( "Code should not access error message when the result is valid." );

				return this._error;
			}
		}

		/// <summary>
		/// Converts this <see cref="Result{T}"/> to <see cref="Maybe{T}"/>, 
		/// using the <paramref name="converter"/> to perform the value conversion.
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The reflector.</param>
		/// <returns><see cref="Maybe{T}"/> that represents the original value behind the <see cref="Result{T}"/> after the conversion</returns>
		public Maybe< TTarget > ToMaybe< TTarget >( Func< T, TTarget > converter )
		{
			Condition.Requires( converter, "converter" ).IsNotNull();
			if( !this.IsSuccess )
				return Maybe< TTarget >.Empty;

			return converter( this._value );
		}

		/// <summary>
		/// Converts this <see cref="Result{T}"/> to <see cref="Maybe{T}"/>, 
		/// with the original value reference, if there is any.
		/// </summary>
		/// <returns><see cref="Maybe{T}"/> that represents the original value behind the <see cref="Result{T}"/>.</returns>
		public Maybe< T > ToMaybe()
		{
			if( !this.IsSuccess )
				return Maybe< T >.Empty;

			return this._value;
		}

		/// <summary>
		/// Exposes result failure as the exception (providing compatibility, with the exception -expecting code).
		/// </summary>
		/// <param name="exception">The function to generate exception, provided the error string.</param>
		/// <returns>result value</returns>
		public T ExposeException( Func< string, Exception > exception )
		{
			Condition.Requires( exception, "exception" ).IsNotNull();
			if( !this.IsSuccess )
				throw exception( this.Error );

			// abdullin: we can return value here, since failure chain ends here
			return this.Value;
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Result{T}"/>.
		/// </summary>
		/// <param name="error">The error.</param>
		/// <returns>The result of the conversion.</returns>
		/// <exception cref="ArgumentNullException">If value is a null reference type</exception>
		public static implicit operator Result< T >( string error )
		{
			Condition.Requires( error, "error" ).IsNotNull();
			return CreateError( error );
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if( !this.IsSuccess )
				return "<Error: '" + this._error + "'>";

			return "<Value: '" + this._value + "'>";
		}
	}
}