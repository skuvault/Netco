using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Netco.Syntaxis;

namespace Netco.ActionPolicyServices
{
	/// <summary>
	/// Policy that could be applied to delegates to
	/// augment their behavior (i.e. to retry on problems)
	/// </summary>
	[ Serializable ]
	public class ActionPolicyAsync
	{
		private readonly Func< Func< Task >, Task > _policy;

		/// <summary>
		/// Initializes a new instance of the <see cref="ActionPolicyAsync"/> class.
		/// </summary>
		/// <param name="policy">The policy.</param>
		public ActionPolicyAsync( Func< Func< Task >, Task > policy )
		{
			Condition.Requires( policy, "policy" ).IsNotNull();

			this._policy = policy;
		}

		/// <summary>
		/// Performs the specified action within the policy.
		/// </summary>
		/// <param name="action">The action to perform.</param>
		[ DebuggerNonUserCode ]
		public Task Do( Func< Task > action )
		{
			return this._policy( action );
		}

		/// <summary>
		/// Performs the specified action within the policy and returns the result
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="action">The action to perform.</param>
		/// <returns>result returned by <paramref name="action"/></returns>
		[ DebuggerNonUserCode ]
		public async Task< TResult > Get< TResult >( Func< Task< TResult > > action )
		{
			var result = default( TResult );
			await this._policy( async () =>
				{
					result = await action().ConfigureAwait( false );
				} ).ConfigureAwait( false );
			return result;
		}

		/// <summary>
		/// Action policy that does not do anything
		/// </summary>
		public static readonly ActionPolicyAsync Null = new ActionPolicyAsync( action => action() );

		/// <summary> Starts building <see cref="ActionPolicyAsync"/> 
		/// that can handle exceptions, as determined by 
		/// <paramref name="handler"/> </summary>
		/// <param name="handler">The exception handler.</param>
		/// <returns>syntax</returns>
		public static SyntaxAsync< ExceptionHandler > With( ExceptionHandler handler )
		{
			Condition.Requires( handler, "handler" ).IsNotNull();
			return Syntax.ForAsync( handler );
		}

		/// <summary> Starts building <see cref="ActionPolicyAsync"/> 
		/// that can handle exceptions, as determined by 
		/// <paramref name="doWeHandle"/> function</summary>
		/// <param name="doWeHandle"> function that returns <em>true</em> if we can hande the specified exception.</param>
		/// <returns>syntax</returns>
		public static SyntaxAsync< ExceptionHandler > From( Func< Exception, bool > doWeHandle )
		{
			Condition.Requires( doWeHandle, "doWeHandle" ).IsNotNull();

			ExceptionHandler handler = exception => doWeHandle( exception );
			return Syntax.ForAsync( handler );
		}

		/// <summary> Starts building simple <see cref="ActionPolicyAsync"/>
		/// that can handle <typeparamref name="TException"/> </summary>
		/// <typeparam name="TException">The type of the exception to handle.</typeparam>
		/// <returns>syntax</returns>
		public static SyntaxAsync< ExceptionHandler > Handle< TException >()
			where TException : Exception
		{
			return Syntax.ForAsync< ExceptionHandler >( ex => ex is TException );
		}

		/// <summary> Starts building simple <see cref="ActionPolicyAsync"/>
		/// that can handle <typeparamref name="TEx1"/> or <typeparamref name="TEx1"/>
		/// </summary>
		/// <typeparam name="TEx1">The type of the exception to handle.</typeparam>
		/// <typeparam name="TEx2">The type of the exception to handle.</typeparam>
		/// <returns>syntax</returns>
		public static SyntaxAsync< ExceptionHandler > Handle< TEx1, TEx2 >()
			where TEx1 : Exception
			where TEx2 : Exception
		{
			return Syntax.ForAsync< ExceptionHandler >( ex => ( ex is TEx1 ) || ( ex is TEx2 ) );
		}

		/// <summary> Starts building simple <see cref="ActionPolicyAsync"/>
		/// that can handle <typeparamref name="TEx1"/> or <typeparamref name="TEx1"/>
		/// </summary>
		/// <typeparam name="TEx1">The first type of the exception to handle.</typeparam>
		/// <typeparam name="TEx2">The second of the exception to handle.</typeparam>
		/// <typeparam name="TEx3">The third of the exception to handle.</typeparam>
		/// <returns>syntax</returns>
		public static SyntaxAsync< ExceptionHandler > Handle< TEx1, TEx2, TEx3 >()
			where TEx1 : Exception
			where TEx2 : Exception
			where TEx3 : Exception
		{
			return Syntax.ForAsync< ExceptionHandler >( ex => ( ex is TEx1 ) || ( ex is TEx2 ) || ( ex is TEx3 ) );
		}
	}
}