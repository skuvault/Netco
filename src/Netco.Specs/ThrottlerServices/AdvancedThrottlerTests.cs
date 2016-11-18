using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Netco.ThrottlerServices;
using NUnit.Framework;

namespace Netco.Specs.ThrottlerServices
{
	internal class AdvancedThrottlerTestsAsync : AdvancedThrottlerTests
	{
		protected override bool IsAsyncVersion { get; set; } = true;
	}

	internal class AdvancedThrottlerTests : ThrottlerTestsBase
	{
		protected override Func<Throttler> GetThrottlerFunc { get; set; } = () => new Throttler(5, 10, 5);
		protected override Func<ThrottlerAsync> GetThrottlerFuncAsync { get; set; } = () => new ThrottlerAsync(5, 10, 5);
		protected override bool IsAsyncVersion { get; set; } = false;

		[Test]
		public void NoDelayUntilLimitHit()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			//------------ Act
			MakeRequests(5);

			//------------ Assert
			stopwatch.Stop();
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds < 1f);
		}

		[Test]
		public void DelayWhenLimitHit()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			//------------ Act
			MakeRequests(6);

			//------------ Assert
			stopwatch.Stop();
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 10.0);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds <= 10.5);
		}

		[Test]
		public void ReleaseAutomaticallyIfTimePasses()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			Debug.WriteLine("Not throttlered requests");
			MakeRequests(5);

			//------------ Act
			Debug.WriteLine("Throttlered requests");
			MakeRequests(2);

			//------------ Assert
			stopwatch.Stop();
			Debug.WriteLine("TotalSeconds=" + stopwatch.Elapsed.TotalSeconds);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 10.0);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds <= 10.5);
		}

		[Test]
		public void ReleaseAutomaticallyIfTimePasses2()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			Debug.WriteLine("Not throttlered requests");
			MakeRequests(5);

			Task.Delay(10000).Wait();

			//------------ Act
			Debug.WriteLine("Not throttlered requests");
			MakeRequests(2);

			//------------ Assert
			stopwatch.Stop();
			Debug.WriteLine("TotalSeconds=" + stopwatch.Elapsed.TotalSeconds);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 10.0);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds <= 10.5);
		}

		[Test]
		public void ReleaseAutomaticallyIfTimePasses3()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			Debug.WriteLine("Not throttlered requests");
			MakeRequests(5);

			Task.Delay(5000).Wait();

			//------------ Act
			Debug.WriteLine("Throttlered requests on remaining 5 sec");
			MakeRequests(2);

			//------------ Assert
			stopwatch.Stop();
			Debug.WriteLine("TotalSeconds=" + stopwatch.Elapsed.TotalSeconds);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 10.0);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds <= 10.5);
		}

		[Test]
		public void ReleaseAutomaticallyIfTimePasses4()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			Debug.WriteLine("Not throttlered requests");
			MakeRequests(5);

			Task.Delay(10000).Wait();

			//------------ Act
			Debug.WriteLine("Not throttlered requests");
			MakeRequests(5);

			//------------ Assert
			stopwatch.Stop();
			Debug.WriteLine("TotalSeconds=" + stopwatch.Elapsed.TotalSeconds);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 10.0);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds <= 10.5);
		}

		[Test]
		public void ReleaseAutomaticallyIfTimePasses5()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			Debug.WriteLine("Not throttlered requests");
			MakeRequests(5);

			Task.Delay(12000).Wait();

			//------------ Act
			Debug.WriteLine("5 not throttlered and 1 throttlered requests");
			MakeRequests(6);

			//------------ Assert
			stopwatch.Stop();
			Debug.WriteLine("TotalSeconds=" + stopwatch.Elapsed.TotalSeconds);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 22.0);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds <= 22.5);
		}

		[Test]
		public void ReleaseAutomaticallyIfTimePasses6()
		{
			//------------ Arrange
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			//------------ Act
			Debug.WriteLine("5 not throttlered and 9 throttlered requests");
			MakeRequests(14);

			//------------ Assert
			stopwatch.Stop();
			Debug.WriteLine("TotalSeconds=" + stopwatch.Elapsed.TotalSeconds);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 20.0);
			Assert.IsTrue(stopwatch.Elapsed.TotalSeconds <= 20.5);
		}
	}
}