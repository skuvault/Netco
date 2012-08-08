using System;
using System.IO;

namespace Netco.Extensions
{
	///<summary>
	///Adds extensions to simplify working with <see cref="System.Reflection.Assembly"/>.
	///</summary>
	public static class AssemblyExtensions
	{
		/// <summary>
		/// Gets the original location of an assembly (before it was shadow-copied).
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns>Original location of the assembly.</returns>
		/// <remarks>Original location or <see cref="string.Empty"/> if location couldn't be determined (because assembly was loaded
		/// from byte array for example).</remarks>
		public static string GetOriginalLocation( this System.Reflection.Assembly assembly )
		{
			if( assembly == null )
				return string.Empty;
			var codeBase = assembly.CodeBase;
			if( string.IsNullOrEmpty( codeBase ))
 				return string.Empty;

			try
			{
				var uri = new Uri( codeBase );
				return Path.GetDirectoryName( uri.LocalPath );
			}
			catch( UriFormatException )
			{
				return string.Empty;
			}
		}
	}
}