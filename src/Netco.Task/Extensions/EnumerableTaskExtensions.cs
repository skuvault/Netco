using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netco.Task.Extensions
{
	public static class EnumerableTaskExtensions
	{
		/// <summary>
		/// Performs an asynchronous action on each element of enumerable in a batch.
		/// </summary>
		/// <typeparam name="TInput">The type of the input.</typeparam>
		/// <param name="inputEnumerable">The input enumerable.</param>
		/// <param name="batchSize">Size of the batch.</param>
		/// <param name="processor">The processor.</param>
		/// <returns>Task indicating when all action have been performed.</returns>
		public static async System.Threading.Tasks.Task DoInBatchAsync< TInput >( this IEnumerable< TInput > inputEnumerable, int batchSize, Func< TInput, System.Threading.Tasks.Task > processor )
		{
			if( inputEnumerable == null )
				return;

			if( processor is null )
				throw new ArgumentNullException( nameof(processor) );

			if( batchSize < 1 )
				throw new ArgumentOutOfRangeException( nameof(batchSize), $"{nameof(batchSize)} must be at least 1" );

			var processingTasks = new List< System.Threading.Tasks.Task >( batchSize );

			foreach( var input in inputEnumerable )
			{
				processingTasks.Add( processor( input ) );

				if( processingTasks.Count == batchSize ) // batch size reached, wait for completion and process
				{
					await System.Threading.Tasks.Task.WhenAll( processingTasks ).ConfigureAwait( false );
					processingTasks.Clear();
				}
			}

			await System.Threading.Tasks.Task.WhenAll( processingTasks ).ConfigureAwait( false );
		}
		
				/// <summary>
		/// Processes elements asynchronously the in batch of the specified size.
		/// </summary>
		/// <typeparam name="TInput">The type of the input.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="inputEnumerable">The input enumerable.</param>
		/// <param name="batchSize">Size of the batch.</param>
		/// <param name="processor">The processor.</param>
		/// <param name="ignoreNull">
		/// if set to <c>true</c> and <paramref name="processor" /> returns <c>null</c> the result is
		/// ignored.
		/// </param>
		/// <returns>Result of processing.</returns>
		public static async Task< IEnumerable< TResult > > ProcessInBatchAsync< TInput, TResult >( this IEnumerable< TInput > inputEnumerable, int batchSize, Func< TInput, Task< TResult > > processor, bool ignoreNull = true )
		{
			if( inputEnumerable == null )
				return Enumerable.Empty< TResult >();
			if( processor is null )
				throw new ArgumentNullException( nameof(processor) );

			if( batchSize < 1 )
				throw new ArgumentOutOfRangeException( nameof(batchSize), $"{nameof(batchSize)} must be at least 1" );

			var result = new List< TResult >( inputEnumerable.Count() );
			var processingTasks = new List< Task< TResult > >( batchSize );

			foreach( var input in inputEnumerable )
			{
				processingTasks.Add( processor( input ) );

				if( processingTasks.Count == batchSize ) // batch size reached, wait for completion and process
				{
					AddResultToList( await System.Threading.Tasks.Task.WhenAll( processingTasks ).ConfigureAwait( false ), result, ignoreNull );
					processingTasks.Clear();
				}
			}

			AddResultToList( await System.Threading.Tasks.Task.WhenAll( processingTasks ).ConfigureAwait( false ), result, ignoreNull );
			return result;
		}

		private static void AddResultToList< TResult >( IEnumerable< TResult > intermidiateResult, List< TResult > endResult, bool ignoreNull )
		{
			foreach( var value in intermidiateResult )
			{
				if( ignoreNull && Equals( value, default(TResult) ) )
					continue;

				endResult.Add( value );
			}
		}

	}
}