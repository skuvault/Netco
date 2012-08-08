using System;
using System.Threading;
using Netco.Threading;

#if !SILVERLIGHT2

namespace Netco.ActionPolicyServices.Exceptions
{
	internal sealed class CircuitBreakerStateLock : ICircuitBreakerState, IDisposable
	{
		private readonly ICircuitBreakerState _inner;
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		public CircuitBreakerStateLock( ICircuitBreakerState inner )
		{
			this._inner = inner;
		}

		public Exception LastException
		{
			get
			{
				using( this._lock.GetReadLock() )
					return this._inner.LastException;
			}
		}

		public bool IsBroken
		{
			get
			{
				using( this._lock.GetReadLock() )
					return this._inner.IsBroken;
			}
		}

		public void Reset()
		{
			using( this._lock.GetWriteLock() )
				this._inner.Reset();
		}

		public void TryBreak( Exception ex )
		{
			using( this._lock.GetWriteLock() )
				this._inner.TryBreak( ex );
		}

		public void Dispose()
		{
			_lock.Dispose();
		}
	}
}

#endif