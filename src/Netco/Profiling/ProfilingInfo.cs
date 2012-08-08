using System;

namespace Netco.Profiling
{
	/// <summary>
	/// Holds profiling information.
	/// </summary>
	public class ProfilingInfo
	{
		internal ProfilingInfo( string name )
		{
			this._name = name;
			this._startTime = DateTime.Now;
			this._startMemory = Profiler.GetCurrentMemoryGCInMB();
		}

		internal void End()
		{
			this._endTime = DateTime.Now;
			this._endMemory = Profiler.GetCurrentMemoryGCInMB();
		}

		private readonly DateTime _startTime;
		private DateTime _endTime = DateTime.MinValue;

		private readonly long _startMemory;
		private long _endMemory = long.MinValue;
		private readonly string _name;

		/// <summary>
		/// Gets the profiling start time.
		/// </summary>
		/// <value>The profiling start time.</value>
		public DateTime StartTime
		{
			get { return this._startTime; }
		}

		/// <summary>
		/// Gets the profiling start memory.
		/// </summary>
		/// <value>The profiling start memory.</value>
		public long StartMemory
		{
			get { return this._startMemory; }
		}

		/// <summary>
		/// Gets the profiling end time.
		/// </summary>
		/// <value>The profiling end time.</value>
		public DateTime EndTime
		{
			get
			{
				if( this._endTime == DateTime.MinValue )
					this.End();
				return this._endTime;
			}
		}

		/// <summary>
		/// Gets the profiling end memory.
		/// </summary>
		/// <value>The profiling end memory.</value>
		public long EndMemory
		{
			get
			{
				if( this._endMemory == long.MinValue )
					this.End();
				return this._endMemory;
			}
		}

		/// <summary>
		/// Gets the profiling span.
		/// </summary>
		/// <value>The profiling span.</value>
		public TimeSpan Duration
		{
			get { return this.EndTime - this.StartTime; }
		}

		/// <summary>
		/// Gets the memory delta.
		/// </summary>
		/// <value>The memory delta.</value>
		public long MemoryDelta
		{
			get { return this.EndMemory - this.StartMemory; }
		}

		/// <summary>
		/// Gets the profiling name.
		/// </summary>
		/// <value>The profiling name.</value>
		public string Name
		{
			get { return this._name; }
		}
	}
}