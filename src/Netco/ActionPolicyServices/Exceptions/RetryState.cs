using System;
using System.Threading.Tasks;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal sealed class RetryState : IRetryState
	{
		private readonly Action< Exception > _onRetry;

		public RetryState( Action< Exception > onRetry )
		{
			this._onRetry = onRetry;
		}

		bool IRetryState.CanRetry( Exception ex )
		{
			this._onRetry( ex );
			return true;
		}
	}

	internal sealed class RetryStateAsync : IRetryStateAsync
	{
		private readonly Func< Exception, Task > _onRetry;

		public RetryStateAsync( Func< Exception, Task > onRetry )
		{
			this._onRetry = onRetry;
		}

		async Task< bool > IRetryStateAsync.CanRetry( Exception ex )
		{
			await this._onRetry( ex ).ConfigureAwait( false );
			return true;
		}
	}
}