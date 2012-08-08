using System;
using System.ComponentModel;

namespace Netco.Events
{
	/// <summary>
	/// Simplifies raising events.
	/// </summary>
	public static class OnEvent
	{
		/// <summary>
		/// Call to raise generic event.
		/// </summary>
		/// <param name="eventHandler">The event handler.</param>
		/// <param name="source">The source.</param>
		public static void Raise( this EventHandler eventHandler, object source )
		{
			if( eventHandler != null )
				eventHandler( source, new EventArgs() );
		}
		/// <summary>
		/// Call to raise event with specific event arguments.
		/// </summary>
		/// <typeparam name="T">Event arguments type.</typeparam>
		/// <param name="eventHandler">The event handler.</param>
		/// <param name="source">The source.</param>
		/// <param name="e">The event arguments.</param>
		public static void Raise< T >( this EventHandler< T > eventHandler, object source, T e ) where T: EventArgs
		{
			if( eventHandler != null )
				eventHandler( source, e );
		}

		/// <summary>
		/// Call to raise event in response to a property being changed.
		/// </summary>
		/// <param name="eventHandler">The event handler.</param>
		/// <param name="source">The source.</param>
		/// <param name="propertyName">Name of the property.</param>
		public static void Raise( this PropertyChangedEventHandler eventHandler, object source, string propertyName )
		{
			if( eventHandler != null )
				eventHandler( source, new PropertyChangedEventArgs( propertyName )); 
		}
	}
}