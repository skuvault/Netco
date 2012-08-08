using System;
using System.Collections.Generic;
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
}