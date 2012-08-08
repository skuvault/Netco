using System;

namespace Netco.Threading
{
	/// <summary>
	/// Class that allows action to be executed, when it is disposed
	/// </summary>
	[ Serializable ]
	public sealed class DisposableAction : IDisposable
	{
		private readonly Action _action;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableAction"/> class.
		/// </summary>
		/// <param name="action">The action.</param>
		public DisposableAction( Action action )
		{
			this._action = action;
		}

		/// <summary>
		/// Executes the action
		/// </summary>
		public void Dispose()
		{
			this._action();
		}
	}
}