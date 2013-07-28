#pragma warning disable 1998
using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Netco.ActionPolicyServices;

namespace Netco.Specs.ActionPolicyServices
{
	public class ActionPolicyAsyncTests
	{
		[ Test ]
		public void PolicyRetried()
		{
			//------------ Arrange
			var retried = false;
			var callNumber = 0;
			var policy = ActionPolicyAsync.Handle< Exception >().RetryAsync( 2, async ( ex, i ) => retried = true );

			//------------ Act
			var task = policy.Do( async () =>
				{
					callNumber ++;
					await this.TestAsyncMethod( callNumber < 2 );
				} );

			//------------ Assert					
			task.Wait( 5000 );
			retried.Should().BeTrue();
		}

		[ Test ]
		public void PolicyRetriedForever()
		{
			//------------ Arrange
			var retried = 0;
			var callNumber = 0;
			var policy = ActionPolicyAsync.Handle< Exception >().RetryForeverAsync( async ex => retried++ );

			//------------ Act
			var task = policy.Do( async () =>
				{
					callNumber ++;
					await this.TestAsyncMethod( callNumber < 5 );
				} );

			//------------ Assert					
			task.Wait( 5000 );
			retried.Should().Be( 4 );
		}

		[ Test ]
		public async void PolicyThrowsOnExceededRetries()
		{
			//------------ Arrange
			var callNumber = 0;
			var retried = false;
			var policy = ActionPolicyAsync.Handle< Exception >().RetryAsync( 2, async ( ex, i ) => retried = true );

			//------------ Act

			try
			{
				await policy.Do( async () =>
					{
						callNumber ++;
						await this.TestAsyncMethod( callNumber < 5 );
					} );
			}
			catch( Exception exception )
			{
				exception.Should().BeOfType< Exception >();
				exception.Message.Should().Be( "test" );
			}
			//------------ Assert					
			retried.Should().BeTrue();
		}

		[ Test ]
		public void PolicyReturnsResult()
		{
			//------------ Arrange
			var retried = false;
			var callNumber = 0;
			var policy = ActionPolicyAsync.Handle< Exception >().RetryAsync( 2, async ( ex, i ) => retried = true );

			//------------ Act
			var task = policy.Get( async () =>
				{
					callNumber ++;
					await this.TestAsyncMethod( callNumber < 2 );
					return true;
				} );

			//------------ Assert					
			task.Wait( 5000 );
			task.Result.Should().BeTrue();
			retried.Should().BeTrue();
		}

		private async Task TestAsyncMethod( bool throwException )
		{
			if( throwException )
				throw new Exception( "test" );
			await Task.Delay( 0 );
		}
	}
}
#pragma warning restore 1998