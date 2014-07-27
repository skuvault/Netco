using System;
using System.Threading.Tasks;

namespace Netco.Extensions
{
	/// <summary>
	/// Provides helper methods for <see cref="Task"/>
	/// </summary>
	public static class TaskHelper
	{
		/// <summary>
		/// Return task which completes when all supplied tasks complete.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <typeparam name="T2">The type of the 2.</typeparam>
		/// <param name="task1">The task1.</param>
		/// <param name="task2">The task2.</param>
		/// <returns>Task which completes when all supplied tasks complete.</returns>
		public static async Task< Tuple< T1, T2 > > WhenAll< T1, T2 >( Task< T1 > task1, Task< T2 > task2 )
		{
			await Task.WhenAll( task1, task2 ).ConfigureAwait( false );
			return Tuple.Create( task1.Result, task2.Result );
		}

		/// <summary>
		/// Return task which completes when all supplied tasks complete.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <typeparam name="T2">The type of the 2.</typeparam>
		/// <typeparam name="T3">The type of the 3.</typeparam>
		/// <param name="task1">The task1.</param>
		/// <param name="task2">The task2.</param>
		/// <param name="task3">The task3.</param>
		/// <returns>Task which completes when all supplied tasks complete.</returns>
		public static async Task< Tuple< T1, T2, T3 > > WhenAll< T1, T2, T3 >( Task< T1 > task1, Task< T2 > task2, Task< T3 > task3 )
		{
			await Task.WhenAll( task1, task2, task3 ).ConfigureAwait( false );
			return Tuple.Create( task1.Result, task2.Result, task3.Result );
		}

		/// <summary>
		/// Return task which completes when all supplied tasks complete.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <typeparam name="T2">The type of the 2.</typeparam>
		/// <typeparam name="T3">The type of the 3.</typeparam>
		/// <typeparam name="T4">The type of the 4.</typeparam>
		/// <typeparam name="T5">The type of the 5.</typeparam>
		/// <param name="task1">The task1.</param>
		/// <param name="task2">The task2.</param>
		/// <param name="task3">The task3.</param>
		/// <param name="task4">The task4.</param>
		/// <param name="task5">The task5.</param>
		/// <returns>
		/// Task which completes when all supplied tasks complete.
		/// </returns>
		public static async Task< Tuple< T1, T2, T3, T4, T5 > > WhenAll< T1, T2, T3, T4, T5 >( Task< T1 > task1, Task< T2 > task2, Task< T3 > task3, Task< T4 > task4, Task< T5 > task5 )
		{
			await Task.WhenAll( task1, task2, task3, task4, task5 ).ConfigureAwait( false );
			return Tuple.Create( task1.Result, task2.Result, task3.Result, task4.Result, task5.Result );
		}

		/// <summary>
		/// Return task which completes when all supplied tasks complete.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <typeparam name="T2">The type of the 2.</typeparam>
		/// <typeparam name="T3">The type of the 3.</typeparam>
		/// <typeparam name="T4">The type of the 4.</typeparam>
		/// <typeparam name="T5">The type of the 5.</typeparam>
		/// <typeparam name="T6">The type of the 6.</typeparam>
		/// <param name="task1">The task1.</param>
		/// <param name="task2">The task2.</param>
		/// <param name="task3">The task3.</param>
		/// <param name="task4">The task4.</param>
		/// <param name="task5">The task5.</param>
		/// <param name="task6">The task6.</param>
		/// <returns>
		/// Task which completes when all supplied tasks complete.
		/// </returns>
		public static async Task< Tuple< T1, T2, T3, T4, T5, T6 > > WhenAll< T1, T2, T3, T4, T5, T6 >( Task< T1 > task1, Task< T2 > task2, Task< T3 > task3, Task< T4 > task4, Task< T5 > task5, Task< T6 > task6 )
		{
			await Task.WhenAll( task1, task2, task3, task4, task5, task6 ).ConfigureAwait( false );
			return Tuple.Create( task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result );
		}

		/// <summary>
		/// Return task which completes when all supplied tasks complete.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <typeparam name="T2">The type of the 2.</typeparam>
		/// <typeparam name="T3">The type of the 3.</typeparam>
		/// <typeparam name="T4">The type of the 4.</typeparam>
		/// <typeparam name="T5">The type of the 5.</typeparam>
		/// <typeparam name="T6">The type of the 6.</typeparam>
		/// <typeparam name="T7">The type of the 7.</typeparam>
		/// <param name="task1">The task1.</param>
		/// <param name="task2">The task2.</param>
		/// <param name="task3">The task3.</param>
		/// <param name="task4">The task4.</param>
		/// <param name="task5">The task5.</param>
		/// <param name="task6">The task6.</param>
		/// <param name="task7">The task7.</param>
		/// <returns>
		/// Task which completes when all supplied tasks complete.
		/// </returns>
		public static async Task< Tuple< T1, T2, T3, T4, T5, T6, T7 > > WhenAll< T1, T2, T3, T4, T5, T6, T7 >( Task< T1 > task1, Task< T2 > task2, Task< T3 > task3, Task< T4 > task4, Task< T5 > task5, Task< T6 > task6, Task< T7 > task7 )
		{
			await Task.WhenAll( task1, task2, task3, task4, task5, task6, task7 ).ConfigureAwait( false );
			return Tuple.Create( task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result );
		}

		/// <summary>
		/// Return task which completes when all supplied tasks complete.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <typeparam name="T2">The type of the 2.</typeparam>
		/// <typeparam name="T3">The type of the 3.</typeparam>
		/// <typeparam name="T4">The type of the 4.</typeparam>
		/// <typeparam name="T5">The type of the 5.</typeparam>
		/// <typeparam name="T6">The type of the 6.</typeparam>
		/// <typeparam name="T7">The type of the 7.</typeparam>
		/// <typeparam name="T8">The type of the 8.</typeparam>
		/// <param name="task1">The task1.</param>
		/// <param name="task2">The task2.</param>
		/// <param name="task3">The task3.</param>
		/// <param name="task4">The task4.</param>
		/// <param name="task5">The task5.</param>
		/// <param name="task6">The task6.</param>
		/// <param name="task7">The task7.</param>
		/// <param name="task8">The task8.</param>
		/// <returns>
		/// Task which completes when all supplied tasks complete.
		/// </returns>
		public static async Task< Tuple< T1, T2, T3, T4, T5, T6, T7, Tuple< T8 > > > WhenAll< T1, T2, T3, T4, T5, T6, T7, T8 >( Task< T1 > task1, Task< T2 > task2, Task< T3 > task3, Task< T4 > task4, Task< T5 > task5, Task< T6 > task6, Task< T7 > task7, Task< T8 > task8 )
		{
			await Task.WhenAll( task1, task2, task3, task4, task5, task6, task7, task8 ).ConfigureAwait( false );
			return Tuple.Create( task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result );
		}
	}
}