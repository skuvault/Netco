using System;

namespace Netco.ActionPolicyServices
{
	/// <summary> This delegate represents <em>catch</em> block
	/// </summary>
	/// <param name="ex">Exception to handle</param>
	/// <returns><em>true</em> if we can handle exception</returns>
	public delegate bool ExceptionHandler( Exception ex );
}