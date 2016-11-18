using System;
using System.Collections.Generic;
using NLog;

namespace Netco.Logging.NLogIntegration
{
	internal class NLogLogger: ILogger
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
			this._logger.Trace( exception, message );
		}

		public void Trace( string format, params object[] args )
		{
			this._logger.Trace( format, args );
		}

		public void Trace( Exception exception, string format, params object[] args )
		{
			this._logger.Trace( exception, string.Format( format, args ) );
		}

		public void Debug( string message )
		{
			this._logger.Debug( message );
		}

		public void Debug( Exception exception, string message )
		{
			this._logger.Debug( exception, message );
		}

		public void Debug( string format, params object[] args )
		{
			this._logger.Debug( format, args );
		}

		public void Debug( Exception exception, string format, params object[] args )
		{
			this._logger.Debug( exception, string.Format( format, args ) );
		}

		public void Info( string message )
		{
			this._logger.Info( message );
		}

		public void Info( Exception exception, string message )
		{
			this._logger.Info( exception, message );
		}

		public void Info( string format, params object[] args )
		{
			this._logger.Info( format, args );
		}

		public void Info( Exception exception, string format, params object[] args )
		{
			this._logger.Info( exception, string.Format( format, args ) );
		}

		public void Warn( string message )
		{
			this._logger.Warn( message );
		}

		public void Warn( Exception exception, string message )
		{
			this._logger.Warn( exception, message );
		}

		public void Warn( string format, params object[] args )
		{
			this._logger.Warn( format, args );
		}

		public void Warn( Exception exception, string format, params object[] args )
		{
			this._logger.Warn( exception, string.Format( format, args ) );
		}

		public void Error( string message )
		{
			this._logger.Error( message );
		}

		public void Error( Exception exception, string message )
		{
			this._logger.Error( exception, message );
		}

		public void Error( string format, params object[] args )
		{
			this._logger.Error( format, args );
		}

		public void Error( Exception exception, string format, params object[] args )
		{
			this._logger.Error( exception, string.Format( format, args ) );
		}

		public void Fatal( string message )
		{
			this._logger.Fatal( message );
		}

		public void Fatal( Exception exception, string message )
		{
			this._logger.Fatal( exception, message );
		}

		public void Fatal( string format, params object[] args )
		{
			this._logger.Fatal( format, args );
		}

		public void Fatal( Exception exception, string format, params object[] args )
		{
			this._logger.Fatal( exception, string.Format( format, args ) );
		}
	}

	/// <summary>
	/// Returns logger
	/// </summary>
	public class NLogLoggerFactory: ILoggerFactory
	{
		private readonly Dictionary< Type, ILogger > _typeLoggers = new Dictionary< Type, ILogger >();
		private readonly Dictionary< string, ILogger > _loggers = new Dictionary< string, ILogger >();

		/// <summary>
		/// Gets the logger to log message for the specified type.
		/// </summary>
		/// <param name="objectToLogType">Type of the object to log.</param>
		/// <returns>
		/// Logger to log messages for the specified type.
		/// </returns>
		public ILogger GetLogger( Type objectToLogType )
		{
			if( !this._typeLoggers.ContainsKey( objectToLogType ) )
				this._typeLoggers[ objectToLogType ] = new NLogLogger( LogManager.GetLogger( objectToLogType.Name ) );
			return this._typeLoggers[ objectToLogType ];
		}

		/// <summary>
		/// Gets the logger to log message with the specified name.
		/// </summary>
		/// <param name="loggerName">The logger name.</param>
		/// <returns>
		/// Named logger to log messages.
		/// </returns>
		public ILogger GetLogger( string loggerName )
		{
			if( !this._loggers.ContainsKey( loggerName ) )
				this._loggers[ loggerName ] = new NLogLogger( LogManager.GetLogger( loggerName ) );
			return this._loggers[ loggerName ];
		}
	}
}