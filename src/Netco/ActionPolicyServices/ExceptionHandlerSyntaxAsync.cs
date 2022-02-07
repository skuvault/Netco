using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Netco.ActionPolicyServices.Exceptions;
using Netco.Syntaxis;

namespace Netco.ActionPolicyServices
{
	/// <summary>
	/// Fluent API for defining <see cref="ActionPolicyAsync" />
	/// that allows to handle exceptions.
	/// </summary>
	public static class ExceptionHandlerSyntaxAsync
	{
#if !SILVERLIGHT2

		/// <summary>
		///     <para>
		///     Builds the policy that will "break the circuit" after <paramref name="countBeforeBreaking" />
		///     exceptions that could be handled by the <paramref name="syntax" /> being built. The circuit
		///     stays broken for the <paramref name="duration" />. Any attempt to
		///     invoke method within the policy, while the circuit is broken, will immediately re-throw
		///     the last exception.
		///     </para>
		///     <para>
		///     If the action fails within the policy after the block period, then the breaker
		///     is blocked again for the next <paramref name="duration" />.
		///     It will be reset, otherwise.
		///     </para>
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="duration">How much time the breaker will stay open before resetting</param>
		/// <param name="countBeforeBreaking">How many exceptions are needed to break the circuit</param>
		/// <returns>shared policy instance</returns>
		/// <remarks>(see "ReleaseIT!" for the details)</remarks>
		public static ActionPolicyWithStateAsync CircuitBreaker( this SyntaxAsync< ExceptionHandler > syntax, TimeSpan duration, int countBeforeBreaking )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( duration == default )
				throw new ArgumentException( nameof(duration), $"{nameof(duration)} must not be default value" );

			if( countBeforeBreaking <= 0 )
				throw new ArgumentException( nameof(countBeforeBreaking), $"{nameof(countBeforeBreaking)} must by greater than 0" );

			var state = new CircuitBreakerState( duration, countBeforeBreaking );
			var syncLock = new CircuitBreakerStateLock( state );
			return new ActionPolicyWithStateAsync( action => CircuitBreakerPolicy.ImplementationAsync( action, syntax.Target, syncLock ) );
		}
#endif

		/// <summary>
		/// Builds <see cref="ActionPolicyAsync" /> that will retry exception handling
		/// for a couple of times before giving up.
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="retryCount">The retry count.</param>
		/// <returns>reusable instance of policy</returns>
		public static ActionPolicyAsync Retry( this SyntaxAsync< ExceptionHandler > syntax, int retryCount )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			IRetryState stateFactory()
			{
				return new RetryStateWithCount( retryCount, DoNothing2 );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}

		/// <summary>
		/// Builds <see cref="ActionPolicyAsync" /> that will retry exception handling
		/// for a couple of times before giving up.
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="retryCount">The retry count.</param>
		/// <param name="onRetry">
		/// The action to perform on retry (i.e.: write to log).
		/// First parameter is the exception and second one is its number in sequence.
		/// </param>
		/// <returns>reusable policy instance </returns>
		public static ActionPolicyAsync Retry( this SyntaxAsync< ExceptionHandler > syntax, int retryCount, Action< Exception, int > onRetry )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( onRetry is null )
				throw new ArgumentNullException( nameof(onRetry) );

			if( retryCount <= 0 )
				throw new ArgumentException( nameof(retryCount), $"{nameof(retryCount)} must by greater than 0" );

			IRetryState stateFactory()
			{
				return new RetryStateWithCount( retryCount, onRetry );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}

		/// <summary>
		/// Builds <see cref="ActionPolicyAsync" /> that will retry exception handling
		/// for a couple of times before giving up.
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="retryCount">The retry count.</param>
		/// <param name="onRetry">
		/// The action to perform on retry (i.e.: write to log).
		/// First parameter is the exception and second one is its number in sequence.
		/// </param>
		/// <returns>reusable policy instance </returns>
		public static ActionPolicyAsync RetryAsync( this SyntaxAsync< ExceptionHandler > syntax, int retryCount, Func< Exception, int, Task > onRetry )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( onRetry is null )
				throw new ArgumentNullException( nameof(onRetry) );

			if( retryCount <= 0 )
				throw new ArgumentException( nameof(retryCount), $"{nameof(retryCount)} must by greater than 0" );

			IRetryStateAsync stateFactory()
			{
				return new RetryStateWithCountAsync( retryCount, onRetry );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}

		/// <summary> Builds <see cref="ActionPolicyAsync" /> that will keep retrying forever </summary>
		/// <param name="syntax">The syntax to extend.</param>
		/// <param name="onRetry">The action to perform when the exception could be retried.</param>
		/// <returns> reusable instance of policy</returns>
		public static ActionPolicyAsync RetryForever( this SyntaxAsync< ExceptionHandler > syntax, Action< Exception > onRetry )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( onRetry is null )
				throw new ArgumentNullException( nameof(onRetry) );

			var state = new RetryState( onRetry );
			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, () => state ) );
		}

		/// <summary> Builds <see cref="ActionPolicyAsync" /> that will keep retrying forever </summary>
		/// <param name="syntax">The syntax to extend.</param>
		/// <param name="onRetry">The action to perform when the exception could be retried.</param>
		/// <returns> reusable instance of policy</returns>
		public static ActionPolicyAsync RetryForeverAsync( this SyntaxAsync< ExceptionHandler > syntax, Func< Exception, Task > onRetry )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( onRetry is null )
				throw new ArgumentNullException( nameof(onRetry) );

			var state = new RetryStateAsync( onRetry );
			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, () => state ) );
		}

		/// <summary>
		///     <para>
		///     Builds the policy that will keep retrying as long as
		///     the exception could be handled by the <paramref name="syntax" /> being
		///     built and <paramref name="sleepDurations" /> is providing the sleep intervals.
		///     </para>
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="sleepDurations">The sleep durations.</param>
		/// <param name="onRetry">
		/// The action to perform on retry (i.e.: write to log).
		/// First parameter is the exception and second one is the planned sleep duration.
		/// </param>
		/// <returns>new policy instance</returns>
		public static ActionPolicyAsync WaitAndRetry( this SyntaxAsync< ExceptionHandler > syntax, IEnumerable< TimeSpan > sleepDurations, Action< Exception, TimeSpan > onRetry )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( sleepDurations is null )
				throw new ArgumentNullException( nameof(sleepDurations) );

			if( onRetry is null )
				throw new ArgumentNullException( nameof(onRetry) );

			IRetryState stateFactory()
			{
				return new RetryStateWithSleep( sleepDurations, onRetry );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}

		/// <summary>
		///     <para>
		///     Builds the policy that will keep retrying as long as
		///     the exception could be handled by the <paramref name="syntax" /> being
		///     built and <paramref name="sleepDurations" /> is providing the sleep intervals.
		///     </para>
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="sleepDurations">The sleep durations.</param>
		/// <returns>new policy instance</returns>
		public static ActionPolicyAsync WaitAndRetry( this SyntaxAsync< ExceptionHandler > syntax, IEnumerable< TimeSpan > sleepDurations )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( sleepDurations is null )
				throw new ArgumentNullException( nameof(sleepDurations) );

			IRetryState stateFactory()
			{
				return new RetryStateWithSleep( sleepDurations, DoNothing2 );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}

		/// <summary>
		///     <para>
		///     Builds the policy that will keep retrying as long as
		///     the exception could be handled by the <paramref name="syntax" /> being
		///     built and <paramref name="sleepDurations" /> is providing the sleep intervals.
		///     </para>
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="sleepDurations">The sleep durations.</param>
		/// <param name="onRetry">
		/// The action to perform on retry (i.e.: write to log).
		/// First parameter is the exception and second one is the planned sleep duration.
		/// </param>
		/// <returns>new policy instance</returns>
		public static ActionPolicyAsync WaitAndRetryAsync( this SyntaxAsync< ExceptionHandler > syntax, IEnumerable< TimeSpan > sleepDurations, Func< Exception, TimeSpan, Task > onRetry )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( sleepDurations is null )
				throw new ArgumentNullException( nameof(sleepDurations) );

			if( onRetry is null )
				throw new ArgumentNullException( nameof(onRetry) );

			IRetryStateAsync stateFactory()
			{
				return new RetryStateWithSleepAsync( sleepDurations, onRetry );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}

		/// <summary>
		/// Builds the policy that will keep retrying as long as
		/// the exception could be handled by the <paramref name="syntax" /> being
		/// built and <paramref name="sleepDurations" /> is providing the sleep intervals.
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="sleepDurations">The sleep durations.</param>
		/// <param name="onRetry">
		/// The action to perform on retry (i.e.: write to log).
		/// First parameter is the exception and second one is the planned sleep duration.
		/// </param>
		/// <param name="token">The delay cancellation token.</param>
		/// <returns>
		/// new policy instance
		/// </returns>
		public static ActionPolicyAsync WaitAndRetryAsync( this SyntaxAsync< ExceptionHandler > syntax, IEnumerable< TimeSpan > sleepDurations, Func< Exception, TimeSpan, Task > onRetry, CancellationToken token )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( sleepDurations is null )
				throw new ArgumentNullException( nameof(sleepDurations) );

			if( onRetry is null )
				throw new ArgumentNullException( nameof(onRetry) );

			IRetryStateAsync stateFactory()
			{
				return new RetryStateWithSleepAsync( sleepDurations, onRetry, token );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}

		/// <summary>
		///     <para>
		///     Builds the policy that will keep retrying as long as
		///     the exception could be handled by the <paramref name="syntax" /> being
		///     built and <paramref name="sleepDurations" /> is providing the sleep intervals.
		///     </para>
		/// </summary>
		/// <param name="syntax">The syntax.</param>
		/// <param name="sleepDurations">The sleep durations.</param>
		/// <returns>new policy instance</returns>
		public static ActionPolicyAsync WaitAndRetryAsync( this SyntaxAsync< ExceptionHandler > syntax, IEnumerable< TimeSpan > sleepDurations )
		{
			if( syntax is null )
				throw new ArgumentNullException( nameof(syntax) );

			if( sleepDurations is null )
				throw new ArgumentNullException( nameof(sleepDurations) );

			IRetryStateAsync stateFactory()
			{
				return new RetryStateWithSleepAsync( sleepDurations, DoNothing2Async );
			}

			return new ActionPolicyAsync( action => RetryPolicy.ImplementationAsync( action, syntax.Target, stateFactory ) );
		}
		/* Development notes
		 * =================
		 * If a stateful policy is returned by the syntax, 
		 * it must be wrapped with the sync lock
		 */

		private static void DoNothing2( Exception ex, int count )
		{
		}

		private static void DoNothing2( Exception ex, TimeSpan sleep )
		{
		}

		private static Task DoNothing2Async( Exception ex, TimeSpan sleep )
		{
			var task = new TaskCompletionSource< bool >();
			task.SetResult( true );
			return task.Task;
		}
	}
}