﻿using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Netco.Logging;

namespace Netco.WCF
{
	/// <summary>
	/// Inspects WCF messages and logs them into trace channel.
	/// </summary>
	public class LoggingInspector: IClientMessageInspector
	{
		readonly ILogger _logger = NetcoLogger.GetLogger< LoggingInspector >();

		/// <summary>Enables inspection or modification of a message before a request message is sent to a service.</summary>
		/// <param name="request">The message to be sent to the service.</param>
		/// <param name="channel">The  client object channel.</param>
		/// <returns>
		/// The object that is returned as the <c>correlationState</c> argument of the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)"/> method. This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid"/> to ensure that no two <c>correlationState</c> objects are the same.
		/// </returns>
		public object BeforeSendRequest( ref Message request, IClientChannel channel )
		{
			var correlationState = Guid.NewGuid();
			this._logger.Trace( @"Request {{{0}}}:
{1}", correlationState.ToString(), request.ToString() );
			return correlationState;
		}

		/// <summary>
		/// Enables inspection or modification of a message after a reply message is received but prior to passing it back to the client application.
		/// </summary>
		/// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
		/// <param name="correlationState">Correlation state data.</param>
		public void AfterReceiveReply( ref Message reply, object correlationState )
		{
			if( correlationState != null )
				this._logger.Trace( @"Response {{{0}}}:
{1}", correlationState.ToString(), reply.ToString() );
			else
				this._logger.Trace( @"Response:
{0}", reply.ToString() );
		}
	}
}