using System;

namespace Netco.Logging
{
	/// <summary>
	/// Logger interface to use for logging.
	/// </summary>
	/// <remarks>Logging providers are expected to implement this.</remarks>
	public interface ILogger
	{
		/// <summary>
		/// Logs the trace message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Trace( string message );

		/// <summary>
		/// Logs the trace message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		void Trace( Exception exception, string message );

		/// <summary>
		/// Logs the trace message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Trace( string format, params object[] args );

		/// <summary>
		/// Logs the trace message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Trace( Exception exception, string format, params object[] args );

		/// <summary>
		/// Logs the debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Debug( string message );

		/// <summary>
		/// Logs the debug message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		void Debug( Exception exception, string message );

		/// <summary>
		/// Logs the debug message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Debug( string format, params object[] args );

		/// <summary>
		/// Logs the debug message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Debug( Exception exception, string format, params object[] args );

		/// <summary>
		/// Logs the info message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Info( string message );

		/// <summary>
		/// Logs the info message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		void Info( Exception exception, string message );

		/// <summary>
		/// Logs the info message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Info( string format, params object[] args );

		/// <summary>
		/// Logs the info message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Info( Exception exception, string format, params object[] args );

		/// <summary>
		/// Logs the warn message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Warn( string message );

		/// <summary>
		/// Logs the warn message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		void Warn( Exception exception, string message );

		/// <summary>
		/// Logs the warn message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Warn( string format, params object[] args );

		/// <summary>
		/// Logs the warn message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Warn( Exception exception, string format, params object[] args );

		/// <summary>
		/// Logs the error message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Error( string message );

		/// <summary>
		/// Logs the error message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		void Error( Exception exception, string message );

		/// <summary>
		/// Logs the error message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Error( string format, params object[] args );

		/// <summary>
		/// Logs the error message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Error( Exception exception, string format, params object[] args );

		/// <summary>
		/// Logs the fatal message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Fatal( string message );

		/// <summary>
		/// Logs the fatal message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		void Fatal( Exception exception, string message );

		/// <summary>
		/// Logs the fatal message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Fatal( string format, params object[] args );

		/// <summary>
		/// Logs the fatal message.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		void Fatal( Exception exception, string format, params object[] args );
	}

	/// <summary>
	/// Logger factory interface. Supplies logger for each log call.
	/// </summary>
	/// <remarks>Needs to be implemented by each separate logger provider.</remarks>
	public interface ILoggerFactory
	{
		/// <summary>
		/// Gets the logger to log message for the specified type.
		/// </summary>
		/// <param name="objectToLogType">Type of the object to log.</param>
		/// <returns>Logger to log messages for the specified type.</returns>
		ILogger GetLogger( Type objectToLogType );

		/// <summary>
		/// Gets the logger to log message for the specified type.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <returns> Logger to log messages for the specified type.</returns>
		ILogger GetLogger( string loggerName );
	}

	/// <summary>
	/// Extends all objects to support logging.
	/// </summary>
	public static class LogExtensions
	{
		/// <summary>
		/// Gets the logger for the specified object.
		/// </summary>
		/// <typeparam name="T">Type of the object for which to get the logger.</typeparam>
		/// <param name="needToLogObj">Object to log for.</param>
		/// <returns>The logger for the specified object.</returns>
		public static ILogger Log< T >( this T needToLogObj )
		{
			return NetcoLogger.GetLogger( typeof( T ) );
		}

		/// <summary>
		/// Gets the logger with the specified logger name.
		/// </summary>
		/// <typeparam name="T">Type of the object for which to get the logger.</typeparam>
		/// <param name="needToLogObj">Object to log for.</param>
		/// <param name="loggerName">Name of the logger.</param>
		/// <returns>The logger with the specified name.</returns>
		public static ILogger Log< T >( this T needToLogObj, string loggerName )
		{
			return NetcoLogger.GetLogger( loggerName );
		}
	}

	/// <summary>
	/// Provides maing logging support.
	/// </summary>
	public static class NetcoLogger
	{
		static NetcoLogger()
		{
			LoggerFactory = new NullLoggerFactory();
		}

		/// <summary>
		/// Gets or sets the logger factory.
		/// </summary>
		/// <value>
		/// The logger factory that will supply the logger.
		/// </value>
		public static ILoggerFactory LoggerFactory { get; set; }

		/// <summary>
		/// Gets the logger.
		/// </summary>
		/// <param name="objectToLogType">Type of the object to log.</param>
		/// <returns>
		/// Logger to log messages for the specified object type.
		/// </returns>
		public static ILogger GetLogger( Type objectToLogType )
		{
			return LoggerFactory.GetLogger( objectToLogType );
		}

		/// <summary>
		/// Gets the logger.
		/// </summary>
		/// <param name="loggerName">Name of the logger.</param>
		/// <returns>
		/// Logger to log messages for the specified object type.
		/// </returns>
		public static ILogger GetLogger( string loggerName )
		{
			return LoggerFactory.GetLogger( loggerName );
		}
	}
}