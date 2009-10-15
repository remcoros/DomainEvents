namespace RawSoft.DomainEvents
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// A store for callbacks
	/// </summary>
	public interface ICallbackStore
	{
		/// <summary>
		/// Gets the callbacks in this store.
		/// </summary>
		/// <value>The callbacks.</value>
		IEnumerable<Delegate> Callbacks { get; }

		/// <summary>
		/// Adds the specified callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		void Add<TEvent>(Action<TEvent> callback);

		/// <summary>
		/// Removes the specified callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		void Remove<TEvent>(Action<TEvent> callback);

		/// <summary>
		/// Clear all callbacks from this store.
		/// </summary>
		void Clear();
	}
}