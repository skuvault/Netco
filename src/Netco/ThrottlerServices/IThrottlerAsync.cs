using System;
using System.Threading.Tasks;

namespace Netco.ThrottlerServices
{
	/// <summary>
	/// Throttler
	/// </summary>
	public interface IThrottlerAsync
	{
		/// <summary>
		/// </summary>
		/// <param name="funcToThrottle"></param>
		/// <typeparam name="TResult"></typeparam>
		/// <returns></returns>
		/// <exception cref="ThrottlerException"></exception>
		Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> funcToThrottle);
	}
}