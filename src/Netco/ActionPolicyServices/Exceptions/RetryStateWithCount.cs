using System;
using System.Threading.Tasks;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal sealed class RetryStateWithCount : IRetryState
	{
		private int _errorCount;
		private readonly Action< Exception, int > _onRetry;
		private readonly Predicate< int > _canRetry;

		public RetryStateWithCount( int retryCount, Action< Exception, int > onRetry )
		{
			this._onRetry = onRetry;
			this._canRetry = i => this._errorCount <= retryCount;
		}

		public bool CanRetry( Exception ex )
		{
			this._errorCount += 1;

			var result = this._canRetry( this._errorCount );
			if( result )
				this._onRetry( ex, this._errorCount - 1 );
			return result;
		}
	}

	internal sealed class RetryStateWithCountAsync : IRetryStateAsync
	{
		private int _errorCount;
		private readonly Func< Exception, int, Task > _onRetry;
		private readonly Predicate< int > _canRetry;

		public RetryStateWithCountAsync( int retryCount, Func< Exception, int, Task > onRetry )
		{
			this._onRetry = onRetry;
			this._canRetry = i => this._errorCount <= retryCount;
		}

		public async Task< bool > CanRetry( Exception ex )
		{
			this._errorCount += 1;

			var result = this._canRetry( this._errorCount );
			if( result )
				await this._onRetry( ex, this._errorCount - 1 ).ConfigureAwait( false );
			return result;
		}
	}
}