using System;

namespace Netco.ActionPolicyServices.Exceptions
{
	sealed class RetryStateWithCount : IRetryState
	{
		int _errorCount;
		readonly Action<Exception, int> _onRetry;
		readonly Predicate<int> _canRetry;

		public RetryStateWithCount(int retryCount, Action<Exception, int> onRetry)
		{
			this._onRetry = onRetry;
			this._canRetry = i => this._errorCount <= retryCount;
		}

		public bool CanRetry(Exception ex)
		{
			this._errorCount += 1;

			bool result = this._canRetry(this._errorCount);
			if (result)
			{
				this._onRetry(ex, this._errorCount - 1);
			}
			return result;
		}
	}
}