using System;
using System.ServiceModel.Configuration;

namespace Netco.WCF
{
	/// <summary>
	/// Connection point for logging behavior.
	/// </summary>
	/// <example>
	/// Configuration example for app.config (insert into basicHttpBinding)
	/// <code>
	/// <![CDATA[
	///	<client>
	///		<endpoint address="http://serviceURL" 
	///			binding="basicHttpBinding" 
	///			bindingConfiguration="serviceConfig" 
	///			contract="serviceContract"     
	///			name="serviceName"
	///			behaviorConfiguration="loggingEndpointBehavior" />
	///	</client>
	///	<extensions>
	///		<behaviorExtensions>
	///			<add name="clientLogging" type="Netco.WCF.ClientLoggingBehaviorExtensionElement, Netco"/>
	///		</behaviorExtensions>
	///	</extensions>
	///	<behaviors>
	///		<endpointBehaviors>
	///			<behavior name="loggingEndpointBehavior">
	///				<clientLogging />
	///			</behavior>
	///		</endpointBehaviors>
	///	</behaviors>
	/// ]]>
	/// </code>
	/// </example>
	public class ClientLoggingBehaviorExtensionElement : BehaviorExtensionElement
	{
		/// <summary>Creates a behavior extension based on the current configuration settings.</summary>
		/// <returns>The behavior extension.</returns>
		protected override object CreateBehavior()
		{
			return new ClientLoggingEndpointBehavior();
		}

		/// <summary>Gets the type of behavior.</summary>
		/// <returns>A <see cref="T:System.Type"/>.</returns>
		public override Type BehaviorType
		{
			get { return typeof( ClientLoggingEndpointBehavior ); }
		}
	}
}