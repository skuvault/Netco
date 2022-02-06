using System;

namespace Netco.Monads
{
	/// <summary>
	///     Improved version of the Result[T], that could serve as a basis for it.
	/// </summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <typeparam name="TError">The type of the error.</typeparam>
	/// <remarks>It is to be moved up-stream if found useful in other projects.</remarks>
	public sealed class Result< TValue, TError >: IEquatable< Result< TValue, TError > >
	{
		private readonly TError _error;
		private readonly TValue _value;

		private Result( bool isSuccess, TValue value, TError error )
		{
			this.IsSuccess = isSuccess;
			this._value = value;
			this._error = error;
		}

		/// <summary>
		///     Error message associated with this failure
		/// </summary>
		public TError Error
		{
			get
			{
				if( this.IsSuccess )
					throw new InvalidOperationException( "Code should not access error message when the result is valid." );
				return this._error;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this result is valid.
		/// </summary>
		/// <value><c>true</c> if this result is valid; otherwise, <c>false</c>.</value>
		public bool IsSuccess{ get; }

		/// <summary>
		///     item associated with this result
		/// </summary>
		public TValue Value
		{
			get
			{
				if( !this.IsSuccess )
					throw new InvalidOperationException( "Code should not access value when the result has failed." );
				return this._value;
			}
		}

		/// <summary>
		///     Creates the error result.
		/// </summary>
		/// <param name="error">The error.</param>
		/// <returns>result encapsulating the error value</returns>
		/// <exception cref="ArgumentNullException">if error is a null reference type</exception>
		public static Result< TValue, TError > CreateError( TError error )
		{
			if( error == null )
				throw new ArgumentNullException( nameof(error) );

			return new Result< TValue, TError >( false, default, error );
		}

		/// <summary>
		///     Creates the success result.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>result encapsulating the success value</returns>
		/// <exception cref="ArgumentNullException">if value is a null reference type</exception>
		public static Result< TValue, TError > CreateSuccess( TValue value )
		{
			if( value == null )
				throw new ArgumentNullException( nameof(value) );

			return new Result< TValue, TError >( true, value, default );
		}

		/// <summary>
		///     Performs an implicit conversion from <typeparamref name="TValue" /> to <see cref="Result{TValue,TError}" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		/// <exception cref="ArgumentNullException">If value is a null reference type</exception>
		public static implicit operator Result< TValue, TError >( TValue value )
		{
			if( value == null )
				throw new ArgumentNullException( nameof(value) );
			return CreateSuccess( value );
		}

		/// <summary>
		///     Performs an implicit conversion from <typeparamref name="TError" /> to <see cref="Result{TValue,TError}" />.
		/// </summary>
		/// <param name="error">The error.</param>
		/// <returns>The result of the conversion.</returns>
		/// <exception cref="ArgumentNullException">If value is a null reference type</exception>
		public static implicit operator Result< TValue, TError >( TError error )
		{
			if( error == null )
				throw new ArgumentNullException( nameof(error) );
			return CreateError( error );
		}

		/// <summary>
		///     Applies the specified <paramref name="action" />
		///     to this <see cref="Result{T}" />, if it has value.
		/// </summary>
		/// <param name="action">The action to apply.</param>
		/// <returns>returns same instance for inlining</returns>
		/// <exception cref="ArgumentNullException">if <paramref name="action" /> is null</exception>
		public Result< TValue, TError > Apply( Action< TValue > action )
		{
			if( action is null )
				throw new ArgumentNullException( nameof(action) );

			if( this.IsSuccess )
				action( this._value );

			return this;
		}

		/// <summary>
		///     Combines this <see cref="Result{T}" /> with the result returned
		///     by <paramref name="converter" />.
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <returns>Combined result.</returns>
		public Result< TTarget, TError > Combine< TTarget >( Func< TValue, Result< TTarget, TError > > converter )
		{
			if( converter is null )
				throw new ArgumentNullException( nameof(converter) );

			return this.IsSuccess ? converter( this._value ) : Result< TTarget, TError >.CreateError( this._error );
		}

		/// <summary>
		///     Converts value of this instance
		///     using the provided <paramref name="converter" />
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <returns>Converted result</returns>
		/// <exception cref="ArgumentNullException"> if <paramref name="converter" /> is null</exception>
		public Result< TTarget, TError > Convert< TTarget >( Func< TValue, TTarget > converter )
		{
			if( converter is null )
				throw new ArgumentNullException( nameof(converter) );

			return this.IsSuccess ? converter( this._value ) : Result< TTarget, TError >.CreateError( this._error );
		}

		/// <summary>
		///     Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
		/// </returns>
		public bool Equals( Result< TValue, TError > other )
		{
			if( ReferenceEquals( null, other ) )
				return false;
			if( ReferenceEquals( this, other ) )
				return true;
			return other.IsSuccess.Equals( this.IsSuccess ) && Equals( other._value, this._value ) && Equals( other._error, this._error );
		}

		/// <summary>
		///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
		///     <see cref="T:System.Object" />.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
		/// <returns>
		///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
		///     otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">
		///     The <paramref name="obj" /> parameter is null.
		/// </exception>
		public override bool Equals( object obj )
		{
			if( ReferenceEquals( null, obj ) )
				return false;
			if( ReferenceEquals( this, obj ) )
				return true;
			if( obj.GetType() != typeof( Result< TValue, TError > ) )
				return false;
			return this.Equals( ( Result< TValue, TError > )obj );
		}

		/// <summary>
		///     Exposes result failure as the exception (providing compatibility, with the exception -expecting code).
		/// </summary>
		/// <param name="exception">The function to generate exception, provided the error string.</param>
		/// <returns>result value</returns>
		public TValue ExposeException( Func< TError, Exception > exception )
		{
			if( exception is null )
				throw new ArgumentNullException( nameof(exception) );

			if( !this.IsSuccess )
				throw exception( this.Error );

			return this.Value;
		}

		/// <summary>
		///     Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		///     A hash code for the current <see cref="T:System.Object" />.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var result = this.IsSuccess.GetHashCode();

				// ReSharper disable CompareNonConstrainedGenericWithNull
				result = ( result * 397 ) ^ ( this._value != null ? this._value.GetHashCode() : 1 );
				result = ( result * 397 ) ^ ( this._error != null ? this._error.GetHashCode() : 0 );

				// ReSharper restore CompareNonConstrainedGenericWithNull
				return result;
			}
		}

		/// <summary>
		///     Handles the specified handler.
		/// </summary>
		/// <param name="handler">The handler.</param>
		/// <returns>same instance for the inlining</returns>
		public Result< TValue, TError > Handle( Action< TError > handler )
		{
			if( handler is null )
				throw new ArgumentNullException( nameof(handler) );

			if( !this.IsSuccess )
				handler( this._error );

			return this;
		}

		/// <summary>
		///     Converts this <see cref="Result{T}" /> to <see cref="Maybe{T}" />,
		///     using the <paramref name="converter" /> to perform the value conversion.
		/// </summary>
		/// <typeparam name="TTarget">The type of the target.</typeparam>
		/// <param name="converter">The reflector.</param>
		/// <returns>
		///     <see cref="Maybe{T}" /> that represents the original value behind the <see cref="Result{T}" /> after the
		///     conversion
		/// </returns>
		public Maybe< TTarget > ToMaybe< TTarget >( Func< TValue, TTarget > converter )
		{
			if( converter is null )
				throw new ArgumentNullException( nameof(converter) );

			return this.IsSuccess ? converter( this._value ) : Maybe< TTarget >.Empty;
		}

		/// <summary>
		///     Converts this <see cref="Result{T}" /> to <see cref="Maybe{T}" />,
		///     with the original value reference, if there is any.
		/// </summary>
		/// <returns><see cref="Maybe{T}" /> that represents the original value behind the <see cref="Result{T}" />.</returns>
		public Maybe< TValue > ToMaybe()
		{
			return this.IsSuccess ? this._value : Maybe< TValue >.Empty;
		}

		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if( !this.IsSuccess )
				return "<Error: '" + this._error + "'>";

			return "<Value: '" + this._value + "'>";
		}
	}
}