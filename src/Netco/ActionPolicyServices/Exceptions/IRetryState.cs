using System;
using System.Threading.Tasks;

namespace Netco.ActionPolicyServices.Exceptions
{
	internal interface IRetryState
	{
		bool CanRetry( Exception ex );
	}
	
	internal interface IRetryStateAsync
	{
		Task< bool > CanRetry( Exception ex );
	}
}