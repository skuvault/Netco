using System;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal static class CircuitBreakerPolicy
	{
		internal static void Implementation( Action action, ExceptionHandler canHandle, ICircuitBreakerState breaker )
		{
			if( breaker.IsBroken )
				throw breaker.LastException;

			try
			{
				action();
				breaker.Reset();
			}
			catch( Exception ex )
			{
				if( !canHandle( ex ) )
					throw;
				breaker.TryBreak( ex );
				throw;
			}
		}
	}
}