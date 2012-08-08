using System;
using System.Collections.Generic;
using System.Text;

namespace Netco.Logging
{
	/// <summary>
	/// Sends all log messages to the console.
	/// </summary>
	internal sealed class ConsoleLogger : ILogger
	{
		private readonly Type _type;
		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		public ConsoleLogger( Type type )
		{
			this._type = type;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to separate log entries.
		/// </summary>
		/// <value><c>true</c> if separate log entries with new line; otherwise, <c>false</c>.</value>
		/// <remarks>When entries are separated, they are easier to read, but take up more space.</remarks>
		public bool SeparateLogEntries{ get; set; }

		/// <summary>
		/// Logs the specified message.
		/// </summary>
		/// <param name="messages">All message to log.</param>
		private void LM( params string[] messages  )
		{
			var sb = new StringBuilder();
			for( int i = 0; i < messages.Length; i++ )
			{
				sb.AppendLine( messages[ i ] );
				if( i < messages.Length - 1 )
					sb.Append( "\t" );
			}
			if( SeparateLogEntries )
				sb.AppendLine();

			Console.Write( "{0} - {1}", _type.Name, sb );
		}

		public void Trace( string message )
		{
			LM( message );
		}

		public void Trace( Exception exception, string message )
		{
			LM( message, exception.Message, exception.StackTrace );
		}

		public void Trace( string messageWithFormatting, params object[] args )
		{
			LM( string.Format( messageWithFormatting, args ) );
		}

		public void Trace( Exception exception, string messageWithFormatting, params object[] args )
		{
			LM( string.Format( messageWithFormatting, args ), exception.Message, exception.StackTrace );
		}

		public void Debug( string message )
		{
			LM( message );
		}

		public void Debug( Exception exception, string message )
		{
			LM( message, exception.Message, exception.StackTrace );
		}

		public void Debug( string format, params object[] args )
		{
			LM( string.Format( format, args ) );
		}

		public void Debug( Exception exception, string format, params object[] args )
		{
			LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Info( string message )
		{
			LM( message );
		}

		public void Info( Exception exception, string message )
		{
			LM( message, exception.Message, exception.StackTrace );
		}

		public void Info( string format, params object[] args )
		{
			LM( string.Format( format, args ) );
		}

		public void Info( Exception exception, string format, params object[] args )
		{
			LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Warn( string message )
		{
			LM( message );
		}

		public void Warn( Exception exception, string message )
		{
			LM( message, exception.Message, exception.StackTrace );
		}

		public void Warn( string format, params object[] args )
		{
			LM( string.Format( format, args ) );
		}

		public void Warn( Exception exception, string format, params object[] args )
		{
			LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Error( string message )
		{
			LM( message );
		}

		public void Error( Exception exception, string message )
		{
			LM( message, exception.Message, exception.StackTrace );
		}

		public void Error( string format, params object[] args )
		{
			LM( string.Format( format, args ) );
		}

		public void Error( Exception exception, string format, params object[] args )
		{
			LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Fatal( string message )
		{
			LM( message );
		}

		public void Fatal( Exception exception, string message )
		{
			LM( message, exception.Message, exception.StackTrace );
		}

		public void Fatal( string format, params object[] args )
		{
			LM( string.Format( format, args ) );
		}

		public void Fatal( Exception exception, string format, params object[] args )
		{
			LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}
	}

	/// <summary>
	/// Returns console logger for the specified object type.
	/// </summary>
	public sealed class ConsoleLoggerFactory : ILoggerFactory
	{
		/// <summary>
		/// Gets or sets a value indicating whether to separate log entries.
		/// </summary>
		/// <value><c>true</c> if separate log entries with new line; otherwise, <c>false</c>.</value>
		/// <remarks>When entries are separated, they are easier to read, but take up more space.</remarks>
		public bool SeparateLogEntries{ get; set; }

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
			if( !_loggers.ContainsKey( objectToLogType ) )
			{
				_loggers[ objectToLogType ] = new ConsoleLogger( objectToLogType )
					{ SeparateLogEntries = SeparateLogEntries };
			}
			return _loggers[ objectToLogType ];
		}
	}
}