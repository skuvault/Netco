using System;

namespace Netco.Logging.SerilogIntegration
{
	public class SerilogLogger : ILogger
	{
		public Serilog.ILogger Logger { get; private set; }

		public SerilogLogger(Serilog.ILogger logger)
		{
			Logger = logger;
		}

		public void Trace(string message)
		{
			Logger.Verbose(message);
		}

		public void Trace(Exception exception, string message)
		{
			Logger.Verbose(exception, message);
		}

		public void Trace(string format, params object[] args)
		{
			Logger.Verbose(format, args);
		}

		public void Trace(Exception exception, string format, params object[] args)
		{
			Logger.Verbose(exception, format, args);
		}

		public void Debug(string message)
		{
			Logger.Debug(message);
		}

		public void Debug(Exception exception, string message)
		{
			Logger.Debug(exception, message);
		}

		public void Debug(string format, params object[] args)
		{
			Logger.Debug(format, args);
		}

		public void Debug(Exception exception, string format, params object[] args)
		{
			Logger.Debug(exception, format, args);
		}

		public void Info(string message)
		{
			Logger.Information(message);
		}

		public void Info(Exception exception, string message)
		{
			Logger.Information(exception, message);
		}

		public void Info(string format, params object[] args)
		{
			Logger.Information(format, args);
		}

		public void Info(Exception exception, string format, params object[] args)
		{
			Logger.Information(exception, format, args);
		}

		public void Warn(string message)
		{
			Logger.Warning(message);
		}

		public void Warn(Exception exception, string message)
		{
			Logger.Warning(exception, message);
		}

		public void Warn(string format, params object[] args)
		{
			Logger.Warning(format, args);
		}

		public void Warn(Exception exception, string format, params object[] args)
		{
			Logger.Warning(exception, format, args);
		}

		public void Error(string message)
		{
			Logger.Error(message);
		}

		public void Error(Exception exception, string message)
		{
			Logger.Error(exception, message);
		}

		public void Error(string format, params object[] args)
		{
			Logger.Error(format, args);
		}

		public void Error(Exception exception, string format, params object[] args)
		{
			Logger.Error(exception, format, args);
		}

		public void Fatal(string message)
		{
			Logger.Fatal(message);
		}

		public void Fatal(Exception exception, string message)
		{
			Logger.Fatal(exception, message);
		}

		public void Fatal(string format, params object[] args)
		{
			Logger.Fatal(format, args);
		}

		public void Fatal(Exception exception, string format, params object[] args)
		{
			Logger.Fatal(exception, format, args);
		}
	}
}