using System;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal static class RetryPolicy
	{
		internal static void Implementation( Action action, ExceptionHandler canRetry, Func< IRetryState > stateBuilder )
		{
			var state = stateBuilder();
			while( true )
			{
				try
				{
					action();
					return;
				}
				catch( Exception ex )
				{
					if( !canRetry( ex ) )
						throw;

					if( !state.CanRetry( ex ) )
						throw;
				}
			}
		}
	}
}