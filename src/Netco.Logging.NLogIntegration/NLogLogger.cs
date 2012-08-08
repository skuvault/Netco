using System;
using System.Collections.Generic;
using NLog;

namespace Netco.Logging.NLogIntegration
{
	internal class NLogLogger : ILogger
	{
		private readonly Logger _logger;

		public NLogLogger( Logger logger )
		{
			this._logger = logger;
		}

		public void Trace( string message )
		{
			this._logger.Trace( message );
		}

		public void Trace( Exception exception, string message )
		{
			this._logger.TraceException( message, exception );
		}

		public void Trace( string format, params object[] args )
		{
			this._logger.Trace( format, args );
		}

		public void Trace( Exception exception, string format, params object[] args )
		{
			this._logger.TraceException( string.Format( format, args ), exception );
		}

		public void Debug( string message )
		{
			this._logger.Debug( message );
		}

		public void Debug( Exception exception, string message )
		{
			this._logger.DebugException( message, exception );
		}

		public void Debug( string format, params object[] args )
		{
			this._logger.Debug( format, args );
		}

		public void Debug( Exception exception, string format, params object[] args )
		{
			this._logger.DebugException( string.Format( format, args ), exception );
		}

		public void Info( string message )
		{
			this._logger.Info( message );
		}

		public void Info( Exception exception, string message )
		{
			this._logger.InfoException( message, exception );
		}

		public void Info( string format, params object[] args )
		{
			this._logger.Info( format, args );
		}

		public void Info( Exception exception, string format, params object[] args )
		{
			this._logger.InfoException( string.Format( format, args ), exception );
		}

		public void Warn( string message )
		{
			this._logger.Warn( message );
		}

		public void Warn( Exception exception, string message )
		{
			this._logger.WarnException( message, exception );
		}

		public void Warn( string format, params object[] args )
		{
			this._logger.Warn( format, args );
		}

		public void Warn( Exception exception, string format, params object[] args )
		{
			this._logger.WarnException( string.Format( format, args ), exception );
		}

		public void Error( string message )
		{
			this._logger.Error( message );
		}

		public void Error( Exception exception, string message )
		{
			this._logger.ErrorException( message, exception );
		}

		public void Error( string format, params object[] args )
		{
			this._logger.Error( format, args );
		}

		public void Error( Exception exception, string format, params object[] args )
		{
			this._logger.ErrorException( string.Format( format, args ), exception );
		}

		public void Fatal( string message )
		{
			this._logger.Fatal( message );
		}

		public void Fatal( Exception exception, string message )
		{
			this._logger.FatalException( message, exception );
		}

		public void Fatal( string format, params object[] args )
		{
			this._logger.Fatal( format, args );
		}

		public void Fatal( Exception exception, string format, params object[] args )
		{
			this._logger.FatalException( string.Format( format, args ), exception );
		}
	}

	/// <summary>
	/// Returns logger
	/// </summary>
	public class NLogLoggerFactory : ILoggerFactory
	{
		private readonly Dictionary< Type, ILogger > _loggers = new Dictionary< Type, ILogger >();

		/// <summary>
		/// Gets the logger to log message for the specified type.
		/// </summary>
		/// <param name="objectToLogType">Type of the object to log.</param>
		/// <returns>
		/// Logger to log messages for the specified type.
		/// </returns>
		public ILogger GetLogger( Type objectToLogType )
		{
			if( !this._loggers.ContainsKey( objectToLogType ) )
				this._loggers[ objectToLogType ] = new NLogLogger( LogManager.GetLogger( objectToLogType.Name ) );
			return this._loggers[ objectToLogType ];
		}
	}
}
