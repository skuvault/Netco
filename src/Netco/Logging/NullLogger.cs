using System;

namespace Netco.Logging
{
	/// <summary>
	/// Basic logger that does nothing. No messages will be logged.
	/// </summary>
	internal sealed class NullLogger : ILogger
	{
		public void Trace( string message )
		{
		}

		public void Trace( Exception exception, string message )
		{
		}

		public void Trace( string messageWithFormatting, params object[] args )
		{
		}

		public void Trace( Exception exception, string messageWithFormatting, params object[] args )
		{
		}

		public void Debug( string message )
		{
		}

		public void Debug( Exception exception, string message )
		{
		}

		public void Debug( string format, params object[] args )
		{
		}

		public void Debug( Exception exception, string format, params object[] args )
		{
		}

		public void Info( string message )
		{
		}

		public void Info( Exception exception, string message )
		{
		}

		public void Info( string format, params object[] args )
		{
		}

		public void Info( Exception exception, string format, params object[] args )
		{
		}

		public void Warn( string message )
		{
		}

		public void Warn( Exception exception, string message )
		{
		}

		public void Warn( string format, params object[] args )
		{
		}

		public void Warn( Exception exception, string format, params object[] args )
		{
		}

		public void Error( string message )
		{
		}

		public void Error( Exception exception, string message )
		{
		}

		public void Error( string format, params object[] args )
		{
		}

		public void Error( Exception exception, string format, params object[] args )
		{
		}

		public void Fatal( string message )
		{
		}

		public void Fatal( Exception exception, string message )
		{
		}

		public void Fatal( string format, params object[] args )
		{
		}

		public void Fatal( Exception exception, string format, params object[] args )
		{
		}
	}

	/// <summary>
	/// Returns <see cref="NullLogger"/> for all object types.
	/// </summary>
	public sealed class NullLoggerFactory : ILoggerFactory
	{
		private readonly ILogger _nullLogger = new NullLogger();

		/// <summary>
		/// Gets the logger to log message for the specified type.
		/// </summary>
		/// <param name="objectToLogType">Type of the object to log.</param>
		/// <returns>
		/// Logger to log messages for the specified type.
		/// </returns>
		public ILogger GetLogger( Type objectToLogType )
		{
			return _nullLogger;
		}
	}
}