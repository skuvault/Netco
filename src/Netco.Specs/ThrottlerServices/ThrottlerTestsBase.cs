using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Netco.ThrottlerServices;
using NUnit.Framework;

namespace Netco.Specs.ThrottlerServices
{
	internal abstract class ThrottlerTestsBase
	{
		private int _callCounter;
		private Func<int> _callCounterFunc;
		private Func<Func<int>, int> _throttlerExecute;

		protected Throttler Throttler { get; private set; }
		protected ThrottlerAsync ThrottlerAsync { get; private set; }

		protected abstract Func<Throttler> GetThrottlerFunc { get; set; }
		protected abstract Func<ThrottlerAsync> GetThrottlerFuncAsync { get; set; }
		protected abstract bool IsAsyncVersion { get; set; }

		[SetUp]
		public virtual void Init()
		{
			_callCounter = 0;
			_callCounterFunc = () => _callCounter++;
			if (IsAsyncVersion)
			{
				ThrottlerAsync = GetThrottlerFuncAsync();
				_throttlerExecute = x => ThrottlerAsync.ExecuteAsync(() => Task.FromResult(x())).GetAwaiter().GetResult();
			}
			else
			{
				Throttler = GetThrottlerFunc();
				_throttlerExecute = x => Throttler.Execute(x);
			}
		}

		protected void MakeRequests(int requestsCount)
		{
			for (var i = 1; i <= requestsCount; i++)
			{
				Debug.WriteLine("Request=" + i);
				Assert.AreEqual(_callCounter, _throttlerExecute(_callCounterFunc));
			}
		}
	}
}