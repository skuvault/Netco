﻿using System;

namespace Netco.Logging
{
	/// <summary>
	/// Objects of this class provide ability to observe workflow and log it. 
	/// Such objects can be pushed from on place to another, for example, as method's parameters.
	/// Mark's string representation added to log improves log readability.
	/// </summary>
	public class Mark
	{
		private readonly string _value;
		private readonly Mark _parent;

		/// <summary>
		/// Unique value identifying this mark / workflow / unit in the stack trace
		/// </summary>
		public string MarkValue{ get { return this._value; } }

		/// <summary>
		/// Represents parrent of the mark i.e. parent ubit in the stack trace
		/// </summary>
		public Mark Parent { get { return this._parent; } }

		/// <summary>
		/// Create Mark with specified value without parent
		/// </summary>
		/// <param name="markValue">Unique value identifying this mark</param>
		/// <param name="parent">Unique value identifying parent mark</param>
		public Mark( string markValue, Mark parent = null )
		{
			this._value = markValue;
			this._parent = parent;
		}

		/// <summary>
		/// Create non-unique blank mark
		/// </summary>
		/// <returns></returns>
		public static Mark Blank()
		{
			return new Mark( string.Empty );
		}

		/// <summary>
		/// Create Mark with auto-generated value with or without parent
		/// </summary>
		/// <param name="parent">Parent Mark</param>
		/// <returns></returns>
		public static Mark CreateNew( Mark parent = null )
		{
			return new Mark( Convert.ToBase64String( Guid.NewGuid().ToByteArray() ), parent );
		}

		/// <summary>
		/// Returns Mark's string representation
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Parent == null ? this.MarkValue : this.Parent + "-PARENT-" + this.MarkValue;
		}

		/// <summary>
		/// Returns hash code for the mark
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.MarkValue.GetHashCode() ^ this.Parent.GetHashCode();
		}

		#region Equality members
		/// <summary>
		/// Equals
		/// </summary>
		/// <param name="other">Other Mark</param>
		/// <returns>True if equals, else returns false</returns>
		public bool Equals( Mark other )
		{
			if( ReferenceEquals( null, other ) )
				return false;
			if( ReferenceEquals( this, other ) )
				return true;
			return string.Equals( this.MarkValue, other.MarkValue, StringComparison.OrdinalIgnoreCase ) && this.Parent.Equals( other.Parent );
		}

		/// <summary>
		/// Equals
		/// </summary>
		/// <param name="obj">Other object</param>
		/// <returns>True if equals, else returns false</returns>
		public override bool Equals( object obj )
		{
			if( ReferenceEquals( null, obj ) )
				return false;
			if( ReferenceEquals( this, obj ) )
				return true;
			if( obj.GetType() != this.GetType() )
				return false;
			return this.Equals( ( Mark )obj );
		}
		#endregion
	}

	/// <summary>
	/// Extensions for Mark class
	/// </summary>
	public static class MarkExtensions
	{
		/// <summary>
		/// Check the Mark for blank value
		/// </summary>
		/// <param name="source"></param>
		/// <returns>True if Mark is null or blank</returns>
		public static bool IsBlank( this Mark source )
		{
			return source == null || string.IsNullOrWhiteSpace( source.MarkValue );
		}

		/// <summary>
		/// Creates and returns a Mark with parent or returns null if parrent is null
		/// </summary>
		/// <param name="source"></param>
		/// <returns>Null or new Mark with source as parent</returns>
		public static Mark CreateChildOrNull( this Mark source )
		{
			return source != null ? Mark.CreateNew( source ) : null;
		}

		/// <summary>
		/// Create Mark with parent from source
		/// </summary>
		/// <param name="source">Source, that will be a parent</param>
		/// <returns>If source is not null return new mark with parent, else just new mark without parent</returns>
		public static Mark CreateChild( this Mark source )
		{
			return source != null ? Mark.CreateNew( source ) : Mark.CreateNew();
		}

		/// <summary>
		/// Converts mark to string
		/// </summary>
		/// <param name="source">Mark</param>
		/// <returns>String representation of the mark</returns>
		public static string ToStringSafe( this Mark source )
		{
			return IsBlank( source ) ? string.Empty : source.ToString();
		}
	}
}