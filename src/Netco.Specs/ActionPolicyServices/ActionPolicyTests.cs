using System;
using FluentAssertions;
using NUnit.Framework;
using Netco.ActionPolicyServices;

namespace Netco.Specs.ActionPolicyServices
{
	public class ActionPolicyTests
	{
		[ Test ]
		public void PolicyRetried()
		{
			//------------ Arrange
			var retried = false;
			var callNumber = 0;
			var policy = ActionPolicy.Handle< Exception >().Retry( 2, ( ex, i ) => retried = true );

			//------------ Act
			policy.Do( () =>
				{
					callNumber ++;
					if( callNumber < 2 )
						throw new Exception( "test" );
				} );

			//------------ Assert					
			retried.Should().BeTrue();
		}

		[ Test ]
		public void PolicyThrowsOnExceededRetries()
		{
			//------------ Arrange
			var callNumber = 0;
			var retried = false;
			var policy = ActionPolicy.Handle< Exception >().Retry( 2, ( ex, i ) => retried = true );

			//------------ Act
			policy.Invoking( p => p.Do( () =>
				{
					callNumber ++;
					if( callNumber < 5 )
						throw new Exception( "test" );
				} ) ).Should().Throw< Exception >().WithMessage( "test" );
			//------------ Assert					
			retried.Should().BeTrue();
		}
	}
}