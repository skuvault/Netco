using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Netco.ThrottlerServices
{
	/// <summary>
	/// Throttler
	/// </summary>
	public sealed class Throttler : IThrottler
	{
		private readonly int _maxQuota;
		private int _remainingQuota;
		private readonly Func<int, int> _releasedQuotaCalculator;
		private readonly Func<int, int> _delayCalculator;
		private readonly int _maxRetryCount;
		private readonly string _throttleMessage;

		/// <summary>
		///       Throttler constructor. See code section for details
		/// </summary>
		/// <code>
		/// // Maximum request quota: 20 requests. Restore rate: Five items every second
		/// var throttler = new Throttler( 20, 1, 5 )
		/// 
		/// // Maximum request quota of six and a restore rate of one request every minute
		/// var throttler = new Throttler( 6, 60, 1 )
		/// </code>
		/// <param name="maxQuota">Max quota</param>
		/// <param name="delayInSecondsBeforeRelease">Delay in seconds before release</param>
		/// <param name="itemsCountForRelease">Items count for release. Default is 1</param>
		/// <param name="maxRetryCount">Max Retry Count</param>
		/// <param name="throttleMessage">Throttle Message</param>
		public Throttler(int maxQuota, int delayInSecondsBeforeRelease, int itemsCountForRelease = 1, int maxRetryCount = 10, string throttleMessage = "throttle") :
			this(maxQuota, el => el/delayInSecondsBeforeRelease*itemsCountForRelease, el => delayInSecondsBeforeRelease - el, maxRetryCount, throttleMessage)
		{
		}

		/// <summary>
		///       Throttler constructor
		/// </summary>
		/// <param name="maxQuota">Max quota</param>
		/// <param name="releasedQuotaCalculator">Released Quota Calculator. (elapsedTimeInSeconds)=>itemsCountForRelease</param>
		/// <param name="delayCalculator">Delay Calculator. (elapsedTimeInSeconds)=>delayInSeconds</param>
		/// <param name="maxRetryCount">Max Retry Count</param>
		/// <param name="throttleMessage">Throttle Message</param>
		public Throttler(int maxQuota, Func<int, int> releasedQuotaCalculator, Func<int, int> delayCalculator, int maxRetryCount, string throttleMessage)
		{
			_maxQuota = _remainingQuota = maxQuota;
			_releasedQuotaCalculator = releasedQuotaCalculator;
			_delayCalculator = delayCalculator;
			_maxRetryCount = maxRetryCount;
			_throttleMessage = throttleMessage;
		}

		/// <summary>
		/// </summary>
		/// <param name="funcToThrottle"></param>
		/// <typeparam name="TResult"></typeparam>
		/// <returns></returns>
		/// <exception cref="ThrottlerException"></exception>
		public TResult Execute<TResult>(Func<TResult> funcToThrottle)
		{
			var retryCount = 0;
			while (true)
			{
				try
				{
					return TryExecute(funcToThrottle);
				}
				catch (Exception ex)
				{
					if (!IsExceptionFromThrottling(ex))
						throw;

					if (retryCount >= _maxRetryCount)
						throw new ThrottlerException("Throttle max retry count reached", ex);

					_remainingQuota = 0;
					_requestTimer.Restart();
					Delay(0);
					retryCount++;
					// try again through loop
				}
			}
		}

		private bool IsExceptionFromThrottling(Exception exception)
		{
			var x = exception;

			while (x != null)
			{
				if (!string.IsNullOrWhiteSpace(x.Message) && (x.Message.IndexOf(_throttleMessage, StringComparison.OrdinalIgnoreCase) >= 0))
					return true;

				x = x.InnerException;
			}

			return false;
		}

		private TResult TryExecute<TResult>(Func<TResult> funcToThrottle)
		{
			WaitIfNeeded();
			var result = funcToThrottle();
			SubtractQuota();
			return result;
		}

		private void WaitIfNeeded()
		{
			UpdateRequestQuoteFromTimer();

			if (_remainingQuota != 0)
				return;

#if DEBUG
			Debug.WriteLine("[WaitIfNeeded] _remainingQuota=0");
#endif

			Delay();
			UpdateRequestQuoteFromTimer();
		}

		private void Delay()
		{
			var totalSeconds = _requestTimer.Elapsed.TotalSeconds;
			var elapsed = (int) Math.Floor(totalSeconds);
			Delay(elapsed);
		}

		private void Delay(int elapsedTimeInSeconds)
		{
			var delayInSeconds = _delayCalculator(elapsedTimeInSeconds);
			if (delayInSeconds <= 0)
				return;

#if DEBUG
			Debug.WriteLine("[Delay] elapsedTimeInSeconds={0}, delayInSeconds={1}", elapsedTimeInSeconds, delayInSeconds);
#endif
			Task.Delay(delayInSeconds*1000).Wait();
		}

		private void UpdateRequestQuoteFromTimer()
		{
			if (!_requestTimer.IsRunning || (_remainingQuota == _maxQuota))
				return;

			var totalSeconds = _requestTimer.Elapsed.TotalSeconds;
			var elapsedTimeInSeconds = (int) Math.Floor(totalSeconds);

			var quotaReleased = _releasedQuotaCalculator(elapsedTimeInSeconds);
			if (quotaReleased == 0)
				return;

			_remainingQuota = Math.Min(_remainingQuota + quotaReleased, _maxQuota);
			_requestTimer.Reset();

#if DEBUG
			Debug.WriteLine("[UpdateRequestQuoteFromTimer] elapsedTimeInSeconds={0}, quotaReleased={1}, _remainingQuota={2}", elapsedTimeInSeconds, quotaReleased, _remainingQuota);
#endif
		}

		private void SubtractQuota()
		{
			_remainingQuota--;
			if (_remainingQuota < 0)
				_remainingQuota = 0;
			_requestTimer.Start();

#if DEBUG
			Debug.WriteLine("[SubtractQuota] _remainingQuota={0}", _remainingQuota);
#endif
		}

		private readonly Stopwatch _requestTimer = new Stopwatch();
	}
}