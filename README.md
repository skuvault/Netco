Netco
=====
The project is just a collection of generic classes which can be used in any project with the goal to simplify and speed up development.

Logging
----------
In most cases it's possible to just do `this.Log().Trace( "Trace message" )`. Log is an object extension method which relies on `NetcoLogger` to fetch correct logger for the object type

`NetcoLogger.LoggerFactory` is a property which allows to configure which logging platform to use. By default it's `NullLogger`, which does nothing.

`ConsoleLogger` will log everything to the console.
`NLogLogger` (which resides in **Netco.Logging.NLogIntegration** project) will use [NLog](http://nlog-project.org/) for logging.

Profiling
---------
Related to logging is `Profiler` class. It relies on logging to report profiling results.

`Profiler.Start( "profile name" )` will start profiling with the specified name (which will be reported in the log). Several profilers can be started at the same time.

`Profiler.End()` and `Profiler.End( "message" )` will end the last started profiling and return `ProfilingInfo` with info about the run (time and memore delta). Results will also be logged to Trace output automatically (if `Profiler.EnableLogging` is `true`).

`Profiler.EnableProfiling` indicates whether profiling is enabled.

Action Policy
---------------
This comes directly from [Lokad.Shared](https://github.com/lokad/lokad-shared-libraries/#readme) project. See [Action Policy application block](http://abdullin.com/journal/2008/12/1/net-exception-handling-action-policies-application-block.html) for more info.

Maybe Monad
-----------------
Again, this comes from [Lokad.Shared](https://github.com/lokad/lokad-shared-libraries/#readme) project. There's a twist.  See [Maybe Monads](http://abdullin.com/journal/2009/10/6/zen-development-practices-c-maybe-monad.html) for more info.

Any 'Maybe' monad can have Metadata added to it.

Events
--------
Generic `OnEvent.Raise` extension method can be used to fire events in more convenient manner (e.g. `OnClick.Raise( args )`).

WCF Logging
----------------
`LoggingInspector`, `ClientLoggingEndpointBehavior` and `ClientLoggingBehaviorExtensionElement` all deal with logging request and response XML which is used by WCF for WebServices connections.

You will need to configure your .config file to properly log WCF requests / responses:

     <client>
	    <endpoint address="http://serviceURL" 
	 		binding="basicHttpBinding" 
	 		bindingConfiguration="serviceConfig" 
	 		contract="serviceContract"     
	 		name="serviceName"
	 		behaviorConfiguration="loggingEndpointBehavior" />
	 </client>
	 <extensions>
	 	<behaviorExtensions>
	 		<add name="clientLogging" type="Netco.WCF.ClientLoggingBehaviorExtensionElement, Netco"/>
	 	</behaviorExtensions>
	 </extensions>
	 <behaviors>
	 	<endpointBehaviors>
	 		<behavior name="loggingEndpointBehavior">
	 			<clientLogging />
	 		</behavior>
	 	</endpointBehaviors>
	 </behaviors>
	 
Post submitting
-----------------
`Netco.Net.PostSubmitter` is a helper class to simplify sending POST requests to a server.

It simplifies sending values with the POST, dealing with errors, timeouts, etc.
