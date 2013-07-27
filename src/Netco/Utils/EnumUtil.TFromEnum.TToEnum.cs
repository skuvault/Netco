using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netco.Monads;

namespace Netco.Utils
{
	/// <summary>
	/// Ensures that enums can be converted between each other
	/// </summary>
	/// <typeparam name="TFromEnum">The type of from enum.</typeparam>
	/// <typeparam name="TToEnum">The type of to enum.</typeparam>
	internal static class EnumUtil< TFromEnum, TToEnum >
		where TFromEnum : struct, IComparable
		where TToEnum : struct, IComparable
	{
		private static readonly IDictionary< TFromEnum, TToEnum > _enums;
		private static readonly TFromEnum[] _unmatched;

		static EnumUtil()
		{
			var fromEnums = EnumUtil.GetValues< TFromEnum >();
			_enums = new Dictionary< TFromEnum, TToEnum >( fromEnums.Length, EnumUtil< TFromEnum >.Comparer );
			var unmatched = new List< TFromEnum >();

			foreach( var fromEnum in fromEnums )
			{
				var @enum = fromEnum;
				MaybeParse
					.Enum< TToEnum >( fromEnum.ToString() )
					.Handle( () => unmatched.Add( @enum ) )
					.Apply( match => _enums.Add( @enum, match ) );
			}

			_unmatched = unmatched.ToArray();
		}

		public static TToEnum Convert( TFromEnum from )
		{
			ThrowIfInvalid();
			return _enums[ from ];
		}

		private static void ThrowIfInvalid()
		{
			if( _unmatched.Length > 0 )
			{
				var list = string.Join( ", ", _unmatched.Select( e => e.ToString() ) );
				var message = string.Format( CultureInfo.InvariantCulture,
					"Can't convert from {0} to {1} because of unmatched entries: {2}",
					typeof( TFromEnum ), typeof( TToEnum ), list );
				throw new ArgumentException( message );
			}
		}
	}
}