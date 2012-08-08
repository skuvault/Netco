using System;

namespace Netco.Events
{
	/// <summary>
	/// Generic version of EventArgs to avoid 
	/// defining custom  EventArgs types 
	/// </summary>
	/// <typeparam name="T">Data to pass to event handler.</typeparam>
	/// <remarks>Based on Ayende's Rhino Commons.</remarks>
	public class EventArgs< T > : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="item">The item.</param>
		public EventArgs( T item )
		{
			this._item = item;
		}

		private T _item;

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public T Item
		{
			get { return this._item; }
			set { this._item = value; }
		}
	}
}