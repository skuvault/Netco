using System;
using System.Collections.Generic;

namespace Netco.Monads
{
	/// <summary>
	/// Represents meta data.
	/// </summary>
	[ Serializable ]
	public sealed class MetaData
	{
		private readonly Dictionary< object, object > _data = new Dictionary< object, object >();

		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> with the specified meta key.
		/// </summary>
		public object this[ object metaKey ]
		{
			get { return this._data[ metaKey ]; }
			set { this._data[ metaKey ] = value; }
		}

		/// <summary>
		/// Gets the meta data.
		/// </summary>
		/// <typeparam name="TMetaValue">The type of the meta.</typeparam>
		/// <param name="metaKey">The meta key.</param>
		/// <returns>Meta data casted to the specified type.</returns>
		public TMetaValue GetValue< TMetaValue >( object metaKey )
		{
			return ( TMetaValue )this._data[ metaKey ];
		}

		/// <summary>
		/// Attempts to get meta data.
		/// </summary>
		/// <typeparam name="TMetaValue">The type of the meta.</typeparam>
		/// <param name="metaKey">The meta key.</param>
		/// <returns>Maybe with the specified meta data if meta data was found and successfully casted to the specified type.
		/// Otherwise empty Maybe is returned.</returns>
		public Maybe< TMetaValue > TryGetValue< TMetaValue >( object metaKey )
		{
			object value;
			if( this._data.TryGetValue( metaKey, out value ) )
			{
				if( value is TMetaValue )
					return ( TMetaValue )value;
			}
			return Maybe< TMetaValue >.Empty;
		}
	}
}