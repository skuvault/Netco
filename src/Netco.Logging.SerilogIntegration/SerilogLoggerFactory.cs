using System;
using System.Collections.Generic;

namespace Netco.Logging.SerilogIntegration
{
	public class SerilogLoggerFactory: ILoggerFactory
	{
		private readonly bool _logFullTypeName;
		private readonly Serilog.ILogger _serilogger;

		private readonly Dictionary< Type, ILogger > _typeLoggers = new Dictionary< Type, ILogger >();
		private readonly Dictionary< string, ILogger > _loggers = new Dictionary< string, ILogger >();

		public SerilogLoggerFactory( Serilog.ILogger serilogger, bool logFullTypeName = false )
		{
			if( serilogger == null )
				throw new ArgumentNullException( "serilogger" );

			this._serilogger = serilogger;
			this._logFullTypeName = logFullTypeName;
		}

		public ILogger GetLogger( Type objectToLogType )
		{
			ILogger logger;
			if( this._typeLoggers.TryGetValue( objectToLogType, out logger ) )
				return logger;

			logger = this.CreateLogger( this._serilogger.ForContext( "SourceContext", this._logFullTypeName ? objectToLogType.FullName : objectToLogType.Name ) );
			this._typeLoggers[ objectToLogType ] = logger;
			return logger;
		}

		public ILogger GetLogger( string loggerName )
		{
			ILogger logger;
			if( this._loggers.TryGetValue( loggerName, out logger ) )
				return logger;

			logger = this.CreateLogger( this._serilogger.ForContext( "SourceContext", loggerName ) );
			this._loggers[ loggerName ] = logger;
			return logger;
		}

		public ILogger CreateLogger( Serilog.ILogger logger )
		{
			return new SerilogLogger( logger );
		}

		public void SetLoggerForType< T >( Serilog.ILogger logger )
		{
			this._typeLoggers[ typeof( T ) ] = this.CreateLogger( logger );
		}

		public void SetLoggerForType( Type objectToLogType, Serilog.ILogger logger )
		{
			this._typeLoggers[ objectToLogType ] = this.CreateLogger( logger );
		}

		public void SetLoggerForType( string name, Serilog.ILogger logger )
		{
			this._loggers[ name ] = this.CreateLogger( logger );
		}
	}
}