﻿using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Netco.WCF
{
	/// <summary>
	/// Connection point to WCF to provide logging behavior.
	/// </summary>
	public class ClientLoggingEndpointBehavior : IEndpointBehavior
	{
		/// <summary>Implement to confirm that the endpoint meets some intended criteria.</summary>
		/// <param name="endpoint">The endpoint to validate.</param>
		public void Validate( ServiceEndpoint endpoint )
		{
		}

		/// <summary>Implement to pass data at runtime to bindings to support custom behavior.</summary>
		/// <param name="endpoint">The endpoint to modify.</param>
		/// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
		public void AddBindingParameters( ServiceEndpoint endpoint, BindingParameterCollection bindingParameters )
		{
		}

		/// <summary>Implements a modification or extension of the service across an endpoint.</summary>
		/// <param name="endpoint">The endpoint that exposes the contract.</param>
		/// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
		public void ApplyDispatchBehavior( ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher )
		{
		}

		/// <summary>Implements a modification or extension of the client across an endpoint.</summary>
		/// <param name="endpoint">The endpoint that is to be customized.</param>
		/// <param name="clientRuntime">The client runtime to be customized.</param>
		public void ApplyClientBehavior( ServiceEndpoint endpoint, ClientRuntime clientRuntime )
		{
			var inspector = new LoggingInspector();
			clientRuntime.MessageInspectors.Add( inspector );
		}
	}
}