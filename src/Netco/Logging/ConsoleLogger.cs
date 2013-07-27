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
		private readonly string _name;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
		/// </summary>
		/// <param name="name">The logger name.</param>
		public ConsoleLogger( string name )
		{
			this._name = name;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to separate log entries.
		/// </summary>
		/// <value><c>true</c> if separate log entries with new line; otherwise, <c>false</c>.</value>
		/// <remarks>When entries are separated, they are easier to read, but take up more space.</remarks>
		public bool SeparateLogEntries { get; set; }

		/// <summary>
		/// Logs the specified message.
		/// </summary>
		/// <param name="messages">All message to log.</param>
		private void LM( params string[] messages )
		{
			var sb = new StringBuilder();
			for( var i = 0; i < messages.Length; i++ )
			{
				sb.AppendLine( messages[ i ] );
				if( i < messages.Length - 1 )
					sb.Append( "\t" );
			}
			if( this.SeparateLogEntries )
				sb.AppendLine();

			Console.Write( "{0} - {1}", this._name, sb );
		}

		public void Trace( string message )
		{
			this.LM( message );
		}

		public void Trace( Exception exception, string message )
		{
			this.LM( message, exception.Message, exception.StackTrace );
		}

		public void Trace( string messageWithFormatting, params object[] args )
		{
			this.LM( string.Format( messageWithFormatting, args ) );
		}

		public void Trace( Exception exception, string messageWithFormatting, params object[] args )
		{
			this.LM( string.Format( messageWithFormatting, args ), exception.Message, exception.StackTrace );
		}

		public void Debug( string message )
		{
			this.LM( message );
		}

		public void Debug( Exception exception, string message )
		{
			this.LM( message, exception.Message, exception.StackTrace );
		}

		public void Debug( string format, params object[] args )
		{
			this.LM( string.Format( format, args ) );
		}

		public void Debug( Exception exception, string format, params object[] args )
		{
			this.LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Info( string message )
		{
			this.LM( message );
		}

		public void Info( Exception exception, string message )
		{
			this.LM( message, exception.Message, exception.StackTrace );
		}

		public void Info( string format, params object[] args )
		{
			this.LM( string.Format( format, args ) );
		}

		public void Info( Exception exception, string format, params object[] args )
		{
			this.LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Warn( string message )
		{
			this.LM( message );
		}

		public void Warn( Exception exception, string message )
		{
			this.LM( message, exception.Message, exception.StackTrace );
		}

		public void Warn( string format, params object[] args )
		{
			this.LM( string.Format( format, args ) );
		}

		public void Warn( Exception exception, string format, params object[] args )
		{
			this.LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Error( string message )
		{
			this.LM( message );
		}

		public void Error( Exception exception, string message )
		{
			this.LM( message, exception.Message, exception.StackTrace );
		}

		public void Error( string format, params object[] args )
		{
			this.LM( string.Format( format, args ) );
		}

		public void Error( Exception exception, string format, params object[] args )
		{
			this.LM( string.Format( format, args ), exception.Message, exception.StackTrace );
		}

		public void Fatal( string message )
		{
			this.LM( message );
		}

		public void Fatal( Exception exception, string message )
		{
			this.LM( message, exception.Message, exception.StackTrace );
		}

		public void Fatal( string format, params object[] args )
		{
			this.LM( string.Format( format, args ) );
		}

		public void Fatal( Exception exception, string format, params object[] args )
		{
			this.LM( string.Format( format, args ), exception.Message, exception.StackTrace );
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
		public bool SeparateLogEntries { get; set; }

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
			{
				this._typeLoggers[ objectToLogType ] = new ConsoleLogger( objectToLogType.Name )
					{ SeparateLogEntries = this.SeparateLogEntries };
			}
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
			{
				this._loggers[ loggerName ] = new ConsoleLogger( loggerName )
					{ SeparateLogEntries = this.SeparateLogEntries };
			}
			return this._loggers[ loggerName ];
		}
	}
}