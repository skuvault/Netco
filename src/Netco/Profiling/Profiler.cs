using System;
using System.Collections.Generic;
using System.Diagnostics;
using Netco.Logging;

namespace Netco.Profiling
{
	/// <summary>
	/// Time and memory profiling.
	/// </summary>
	/// <remarks>Profiling works through a stack. <see cref="Start"/> saves current time
	/// and starts profiling. <see cref="End()"/> stops profiling and logs current results.
	/// <see cref="End()"/> must be called in reverse from <see cref="Start"/> order.</remarks>
	public static class Profiler
	{
		static Profiler()
		{
			EnableProfiling = true;
			EnableLogging = true;
		}

		/// <summary>
		/// Gets or sets a value indicating whether profiling is enabled.
		/// </summary>
		/// <value><c>true</c> to enable profiling; otherwise, <c>false</c>.</value>
		/// <remarks>By default profiling is enabled.
		/// <para>This affects prevents profiling from getting started. If any profiling is
		/// taking place it will be ended regardless what this property value is.</para></remarks>
		public static bool EnableProfiling { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to log profiling results.
		/// </summary>
		/// <value><c>true</c> to log results; otherwise, <c>false</c>.</value>
		public static bool EnableLogging { get; set; }

		/// <summary>
		/// Starts profiler.
		/// </summary>
		/// <param name="name">The name of the profile.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
		public static void Start( string name )
		{
			if( string.IsNullOrEmpty( name ) )
				throw new ArgumentNullException( "name", "Profile name cannot be empty" );
			if( !EnableProfiling )
				return;
			lock( _syncLock )
			{
				if( EnableLogging )
					Logger.Trace( "Started profiling " + name );

				_profiles.Push( new ProfilingInfo( name ) );
			}
		}

		/// <summary>
		/// Ends profiling.
		/// </summary>
		/// <returns><see cref="ProfilingInfo"/> holding info about the current profiling.</returns>
		/// <remarks>Logs time profiler ran and memory used delta.</remarks>
		public static ProfilingInfo End()
		{
			return End( string.Empty );
		}

		/// <summary>
		/// Ends profiling with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns><see cref="ProfilingInfo"/> holding info about the current profiling.</returns>
		/// <remarks>Logs time profiler ran and memory used delta.</remarks>
		public static ProfilingInfo End( string message )
		{
			lock( _syncLock )
			{
				if( _profiles.Count == 0 )
					return null;

				var profileInfo = _profiles.Pop();

				profileInfo.End();

				if( EnableLogging )
				{
					var additionalMessage = !string.IsNullOrEmpty( message ) ? " " + message : string.Empty;
					Logger.Trace
						( ProfilingLogMessage + additionalMessage,
							profileInfo.Name, profileInfo.Duration, profileInfo.EndMemory, profileInfo.MemoryDelta );
				}

				return profileInfo;
			}
		}

		private static readonly Stack< ProfilingInfo > _profiles = new Stack< ProfilingInfo >();

		// Lock synchronization object 
		private static readonly object _syncLock = new object();

		#region Misc
		private const long MbDivider = 1024 * 1024;
		private const string ProfilingLogMessage = "{0} profile finished in '{1}'. Current mem is '{2}' MB. Mem delta is '{3}' MB.";

		/// <summary>
		/// Gets the current used memory.
		/// </summary>
		/// <returns>Bytes allocated to the current process.</returns>
		public static long GetCurrentMemory()
		{
			var currentProcess = Process.GetCurrentProcess();
			return currentProcess.WorkingSet64;
		}

		/// <summary>
		/// Gets the current memory in MB.
		/// </summary>
		/// <returns>Megabytes allocated to the current process.</returns>
		public static long GetCurrentMemoryInMB()
		{
			return GetCurrentMemory() / MbDivider;
		}

		/// <summary>
		/// Gets the current memory thought to be allocated by GC.
		/// </summary>
		/// <returns>Bytes allocated by GC.</returns>
		/// <seealso cref="GC.GetTotalMemory"/>
		public static long GetCurrentMemoryGC()
		{
			return GC.GetTotalMemory( true );
		}

		/// <summary>
		/// Gets the current memory thought to be allocated by GC in MB.
		/// </summary>
		/// <returns>Megabytes allocated by GC.</returns>
		/// <seealso cref="GC.GetTotalMemory"/>
		public static long GetCurrentMemoryGCInMB()
		{
			return GetCurrentMemoryGC() / MbDivider;
		}
		#endregion

		#region Logging
		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		private static ILogger Logger
		{
			get { return _logger; }
		}

		private static readonly ILogger _logger = NetcoLogger.GetLogger( typeof( Profiler ) );
		#endregion
	}
}