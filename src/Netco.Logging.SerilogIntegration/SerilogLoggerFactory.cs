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

			_serilogger = serilogger;
			_logFullTypeName = logFullTypeName;
		}

		public ILogger GetLogger( Type objectToLogType )
		{
			ILogger logger;
			if( _typeLoggers.TryGetValue( objectToLogType, out logger ) )
				return logger;

			logger = CreateLogger( _serilogger.ForContext( "SourceContext", _logFullTypeName ? objectToLogType.FullName : objectToLogType.Name ) );
			_typeLoggers[ objectToLogType ] = logger;
			return logger;
		}

		public ILogger GetLogger( string loggerName )
		{
			ILogger logger;
			if( _loggers.TryGetValue( loggerName, out logger ) )
				return logger;

			logger = CreateLogger( _serilogger.ForContext( "SourceContext", loggerName ) );
			_loggers[ loggerName ] = logger;
			return logger;
		}

		public ILogger CreateLogger( Serilog.ILogger logger )
		{
			return new SerilogLogger( logger );
		}

		public void SetLoggerForType< T >( Serilog.ILogger logger )
		{
			_typeLoggers[ typeof( T ) ] = CreateLogger( logger );
		}

		public void SetLoggerForType( Type objectToLogType, Serilog.ILogger logger )
		{
			_typeLoggers[ objectToLogType ] = CreateLogger( logger );
		}

		public void SetLoggerForType( string name, Serilog.ILogger logger )
		{
			_loggers[ name ] = CreateLogger( logger );
		}
	}
}