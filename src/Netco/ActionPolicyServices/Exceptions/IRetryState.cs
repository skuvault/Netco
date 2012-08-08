using System;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal interface IRetryState
	{
		bool CanRetry( Exception ex );
	}
}