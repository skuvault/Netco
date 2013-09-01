using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Netco.Utils;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal sealed class RetryStateWithSleep : IRetryState
	{
		private readonly IEnumerator< TimeSpan > _enumerator;
		private readonly Action< Exception, TimeSpan > _onRetry;

		public RetryStateWithSleep( IEnumerable< TimeSpan > sleepDurations, Action< Exception, TimeSpan > onRetry )
		{
			this._onRetry = onRetry;
			this._enumerator = sleepDurations.GetEnumerator();
		}

		public bool CanRetry( Exception ex )
		{
			if( this._enumerator.MoveNext() )
			{
				var current = this._enumerator.Current;
				this._onRetry( ex, current );
				SystemUtil.Sleep( current );
				return true;
			}
			return false;
		}
	}

	internal sealed class RetryStateWithSleepAsync : IRetryStateAsync
	{
		private readonly IEnumerator< TimeSpan > _enumerator;
		private readonly Func< Exception, TimeSpan, Task > _onRetry;
		private readonly CancellationToken _token;

		public RetryStateWithSleepAsync( IEnumerable< TimeSpan > sleepDurations, Func< Exception, TimeSpan, Task > onRetry )
			: this( sleepDurations, onRetry, CancellationToken.None )
		{
		}
		
		public RetryStateWithSleepAsync( IEnumerable< TimeSpan > sleepDurations, Func< Exception, TimeSpan, Task > onRetry, CancellationToken token )
		{
			this._onRetry = onRetry;
			this._enumerator = sleepDurations.GetEnumerator();
			this._token = token;
		}

		public async Task< bool > CanRetry( Exception ex )
		{
			if( this._enumerator.MoveNext() )
			{
				var current = this._enumerator.Current;
				await this._onRetry( ex, current ).ConfigureAwait( false );
				await Task.Delay( current, _token ).ConfigureAwait( false );
				return true;
			}
			return false;
		}
	}
}