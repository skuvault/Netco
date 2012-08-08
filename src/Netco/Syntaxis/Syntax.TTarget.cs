using System;
using System.ComponentModel;

namespace Netco.Syntaxis
{
	/// <summary>
	/// Helper class for creating fluent APIs
	/// </summary>
	/// <typeparam name="TTarget">underlying type</typeparam>
	[Serializable]
	public sealed class Syntax<TTarget> : Syntax, ISyntax<TTarget>
	{
		readonly TTarget _inner;

		/// <summary>
		/// Initializes a new instance of the <see cref="Syntax{T}"/> class.
		/// </summary>
		/// <param name="inner">The underlying instance.</param>
		public Syntax(TTarget inner)
		{
			this._inner = inner;
		}

		#region ISyntax<TTarget> Members

		/// <summary>
		/// Gets the underlying object.
		/// </summary>
		/// <value>The underlying object.</value>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public TTarget Target
		{
			get { return this._inner; }
		}

		#endregion

		internal static Syntax<TTarget> For(TTarget item)
		{
			return new Syntax<TTarget>(item);
		}
	}
}