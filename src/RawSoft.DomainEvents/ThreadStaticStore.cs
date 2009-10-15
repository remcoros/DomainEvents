namespace RawSoft.DomainEvents
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// A thread static store for callbacks
	/// </summary>
	public class ThreadStaticStore : ICallbackStore
	{
		[ThreadStatic] static IList<Delegate> callbacks;

		#region ICallbackStore Members

		/// <summary>
		/// Gets the callbacks in this store.
		/// </summary>
		/// <value>The callbacks.</value>
		public IEnumerable<Delegate> Callbacks
		{
			get { return callbacks; }
		}

		/// <summary>
		/// Clear all callbacks from this store.
		/// </summary>
		public void Clear()
		{
			callbacks.Clear();
		}

		/// <summary>
		/// Adds the specified callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		public void Add<TEvent>(Action<TEvent> callback)
		{
			if (callbacks == null)
			{
				callbacks = new List<Delegate>();
			}

			callbacks.Add(callback);
		}

		/// <summary>
		/// Removes the specified callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		public void Remove<TEvent>(Action<TEvent> callback)
		{
			callbacks.Remove(callback);
		}

		#endregion
	}
}