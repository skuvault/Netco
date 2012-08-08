using System;
using System.Threading;

#if !SILVERLIGHT2

namespace Netco.Threading
{
	/// <summary>
	/// Helper class that simplifies <see cref="ReaderWriterLockSlim"/> usage
	/// </summary>
	public static class ReaderWriterLockSlimExtensions
	{
		/// <summary>
		/// Gets the read lock object, that is released when the object is disposed.
		/// </summary>
		/// <param name="slimLock">The slim lock object.</param>
		/// <returns></returns>
		public static IDisposable GetReadLock( this ReaderWriterLockSlim slimLock )
		{
			slimLock.EnterReadLock();
			return new DisposableAction( slimLock.ExitReadLock );
		}

		/// <summary>
		/// Gets the write lock, that is released when the object is disposed.
		/// </summary>
		/// <param name="slimLock">The slim lock.</param>
		/// <returns></returns>
		public static IDisposable GetWriteLock( this ReaderWriterLockSlim slimLock )
		{
			slimLock.EnterWriteLock();
			return new DisposableAction( slimLock.ExitWriteLock );
		}

		/// <summary>
		/// Gets the upgradeable read lock, that is released, when the object is disposed
		/// </summary>
		/// <param name="slimLock">The slim lock.</param>
		/// <returns></returns>
		public static IDisposable GetUpgradeableReadLock( this ReaderWriterLockSlim slimLock )
		{
			slimLock.EnterUpgradeableReadLock();
			return new DisposableAction( slimLock.ExitUpgradeableReadLock );
		}
	}
}

#endif