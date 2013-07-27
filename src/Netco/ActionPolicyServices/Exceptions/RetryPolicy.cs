using System;
using System.Threading.Tasks;

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
		
		internal static async Task ImplementationAsync( Func< Task > action, ExceptionHandler canRetry, Func< IRetryState > stateBuilder )
		{
			var state = stateBuilder();
			while( true )
			{
				try
				{
					await action();
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

		internal static async Task ImplementationAsync( Func< Task > action, ExceptionHandler canRetry, Func< IRetryStateAsync > stateBuilder )
		{
			var state = stateBuilder();
			while( true )
			{
				try
				{
					await action();
					return;
				}
				catch( Exception ex )
				{
					if( !canRetry( ex ) )
						throw;

					if( !state.CanRetry( ex ).Result )
						throw;
				}
			}
		}
	}
}