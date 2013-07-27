using System;
using System.ComponentModel;

namespace Netco.Syntaxis
{
	/// <summary>
	/// Helper class for creating fluent APIs
	/// </summary>
	/// <typeparam name="TTarget">underlying type</typeparam>
	/// <remarks>Similar to <see cref="Syntax{T}"/> but for use with async methods.</remarks>
	[ Serializable ]
	public sealed class SyntaxAsync< TTarget > : Syntax, ISyntax< TTarget >
	{
		private readonly TTarget _inner;

		/// <summary>
		/// Initializes a new instance of the <see cref="SyntaxAsync{T}"/> class.
		/// </summary>
		/// <param name="inner">The underlying instance.</param>
		public SyntaxAsync( TTarget inner )
		{
			this._inner = inner;
		}

		#region ISyntax<TTarget> Members
		/// <summary>
		/// Gets the underlying object.
		/// </summary>
		/// <value>The underlying object.</value>
		[ EditorBrowsable( EditorBrowsableState.Advanced ) ]
		public TTarget Target
		{
			get { return this._inner; }
		}
		#endregion

		internal static SyntaxAsync< TTarget > For( TTarget item )
		{
			return new SyntaxAsync< TTarget >( item );
		}
	}
}