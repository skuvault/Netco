using System;

namespace Netco.ThrottlerServices
{
	/// <summary>
	/// Throttler
	/// </summary>
	public interface IThrottler
	{
		/// <summary>
		/// </summary>
		/// <param name="funcToThrottle"></param>
		/// <typeparam name="TResult"></typeparam>
		/// <returns></returns>
		/// <exception cref="ThrottlerException"></exception>
		TResult Execute<TResult>(Func<TResult> funcToThrottle);
	}
}