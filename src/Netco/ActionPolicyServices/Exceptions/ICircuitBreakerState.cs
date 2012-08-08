using System;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal interface ICircuitBreakerState
	{
		Exception LastException { get; }
		bool IsBroken { get; }
		void Reset();
		void TryBreak( Exception ex );
	}
}