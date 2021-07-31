using System;
using System.Collections.Concurrent;

namespace Netco.Logging.SerilogIntegration
{
	public class SerilogLoggerFactory : ILoggerFactory
	{
		private readonly bool _logFullTypeName;
		private readonly Serilog.ILogger _serilogger;

		private readonly ConcurrentDictionary<Type, ILogger> _typeLoggers = new ConcurrentDictionary<Type, ILogger>();
		private readonly ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();

		public SerilogLoggerFactory(Serilog.ILogger serilogger, bool logFullTypeName = false)
		{
			_serilogger = serilogger ?? throw new ArgumentNullException("serilogger");
			_logFullTypeName = logFullTypeName;
		}

		public ILogger GetLogger(Type objectToLogType)
		{
			var logger = this._typeLoggers.GetOrAdd( objectToLogType,
				t => this.CreateLogger( this._serilogger.ForContext( "SourceContext", this._logFullTypeName ? objectToLogType.FullName : objectToLogType.Name ) ) );
			return logger;
		}

		public ILogger GetLogger(string loggerName)
		{
			var logger = this._loggers.GetOrAdd( loggerName, l => this.CreateLogger( this._serilogger.ForContext( "SourceContext", loggerName ) ) );
			return logger;
		}

		public ILogger CreateLogger(Serilog.ILogger logger)
		{
			return new SerilogLogger(logger);
		}

		public void SetLoggerForType<T>(Serilog.ILogger logger)
		{
			this._typeLoggers[ typeof( T ) ] = this.CreateLogger( logger );
		}

		public void SetLoggerForType(Type objectToLogType, Serilog.ILogger logger)
		{
			this._typeLoggers[ objectToLogType ] = this.CreateLogger( logger );
		}

		public void SetLoggerForType(string name, Serilog.ILogger logger)
		{
			this._loggers[ name ] = this.CreateLogger( logger );
		}
	}
}