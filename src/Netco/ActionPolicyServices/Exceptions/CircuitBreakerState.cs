using System;
using Netco.Utils;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal sealed class CircuitBreakerState : ICircuitBreakerState
	{
		public CircuitBreakerState( TimeSpan duration, int exceptionsToBreak )
		{
			this._duration = duration;
			this._exceptionsToBreak = exceptionsToBreak;
			this.Reset();
		}

		private readonly TimeSpan _duration;
		private readonly int _exceptionsToBreak;

		private int _count;
		private DateTime _blockedTill;
		private Exception _lastException;

		public Exception LastException
		{
			get { return this._lastException; }
		}

		public bool IsBroken
		{
			get { return SystemUtil.Now < this._blockedTill; }
		}

		public void Reset()
		{
			this._count = 0;
			this._blockedTill = DateTime.MinValue;
			this._lastException = new InvalidOperationException( "This exception should never be thrown" );
		}

		public void TryBreak( Exception ex )
		{
			this._lastException = ex;

			this._count += 1;
			if( this._count >= this._exceptionsToBreak )
				this.BreakTheCircuit();
		}

		private void BreakTheCircuit()
		{
			this._blockedTill = SystemUtil.Now.Add( this._duration );
		}
	}
}