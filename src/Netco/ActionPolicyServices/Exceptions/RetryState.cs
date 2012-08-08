using System;

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
}