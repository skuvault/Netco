using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netco.Extensions
{
	/// <summary>
	/// Extends enumerables with methods to process collection in pages.
	/// </summary>
	public static class EnumerablePaginationExtensions
	{
		/// <summary>
		/// Processes all items within enumerable by breaking it down into pages.
		/// </summary>
		/// <typeparam name="TData">The type of the data.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="data">The data.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="processor">The processor (which is passed single page).</param>
		/// <returns>Result of pages processing.</returns>
		/// <remarks>All processing is parallelized.</remarks>
		public static IEnumerable< TResult > ProcessWithPages< TData, TResult >( this IEnumerable< TData > data, int pageSize,
			Func< IEnumerable< TData >, IEnumerable< TResult > > processor )
		{
			var dataList = data as IList< TData > ?? data.ToList();
			var pagesCount = dataList.Count / pageSize + ( dataList.Count % pageSize > 0 ? 1 : 0 );

			var pages = new ConcurrentDictionary< int, IEnumerable< TResult > >();

			var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 12 };
			Parallel.For( 0, pagesCount, options, pageNumber =>
				{
					var dataPage = dataList.Skip( pageNumber * pageSize ).Take( pageSize );
					var resultPage = processor( dataPage );
					if( resultPage != null )
						pages[ pageNumber ] = resultPage;
				} );

			// NOTE: force ToList to avoid possible multiple redownloads to reiterate over the list
			return pages.OrderBy( kv => kv.Key ).SelectMany( kv => kv.Value ).ToList();
		}

		/// <summary>
		/// Processes all items within enumerable by breaking it down into pages.
		/// </summary>
		/// <typeparam name="TData">The type of the data.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="data">The data.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="processor">The async processor (which is passed single page).</param>
		/// <returns>Result of pages processing.</returns>
		public static async Task< IEnumerable< TResult > > ProcessWithPagesAsync< TData, TResult >( this IEnumerable< TData > data, int pageSize,
			Func< IEnumerable< TData >, Task< IEnumerable< TResult > > > processor )
		{
			var dataList = data as IList< TData > ?? data.ToList();
			var pagesCount = dataList.Count / pageSize + ( dataList.Count % pageSize > 0 ? 1 : 0 );

			var pageTasks = new Task< IEnumerable< TResult > >[ pagesCount ];

			for( var pageNumber = 0; pageNumber < pagesCount; pageNumber ++ )
			{
				var dataPage = dataList.Skip( pageNumber * pageSize ).Take( pageSize );
				var pageTask = processor( dataPage );
				pageTasks[ pageNumber ] = pageTask;
			}

			var resultPages = await Task.WhenAll( pageTasks );

			// NOTE: force ToList to avoid possible multiple redownloads to reiterate over the list
			return resultPages.Where( r => r != null ).SelectMany( r => r ).ToList();
		}

		/// <summary>
		/// Acts upon data by breaking it down into separate pages.
		/// </summary>
		/// <typeparam name="TData">The type of the data.</typeparam>
		/// <param name="data">The data.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="pageAction">Action for single page.</param>
		public static void DoWithPages< TData >( this IEnumerable< TData > data, int pageSize, Action< IEnumerable< TData > > pageAction )
		{
			var dataList = data as IList< TData > ?? data.ToList();
			var pagesCount = dataList.Count / pageSize + ( dataList.Count % pageSize > 0 ? 1 : 0 );

			var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 12 };
			Parallel.For( 0, pagesCount, options, pageNumber =>
				{
					var dataPage = dataList.Skip( pageNumber * pageSize ).Take( pageSize );
					pageAction( dataPage );
				} );
		}

		/// <summary>
		/// Acts upon data by breaking it down into separate pages (async).
		/// </summary>
		/// <typeparam name="TData">The type of the data.</typeparam>
		/// <param name="data">The data.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="pageAction">Async action for single page.</param>
		public static async Task DoWithPagesAsync< TData >( this IEnumerable< TData > data, int pageSize, Func< IEnumerable< TData >, Task > pageAction )
		{
			var dataList = data as IList< TData > ?? data.ToList();
			var pagesCount = dataList.Count / pageSize + ( dataList.Count % pageSize > 0 ? 1 : 0 );

			var pageTasks = new Task[ pagesCount ];

			for( var pageNumber = 0; pageNumber < pagesCount; pageNumber ++ )
			{
				var dataPage = dataList.Skip( pageNumber * pageSize ).Take( pageSize );
				pageTasks[ pageNumber ] = pageAction( dataPage );
			}

			await Task.WhenAll( pageTasks );
		}
	}
}