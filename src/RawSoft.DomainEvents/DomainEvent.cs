namespace RawSoft.DomainEvents
{
	using System;
	using Microsoft.Practices.ServiceLocation;

	/// <summary>
	/// Use this class to raise events in your domein.
	/// It uses the common service locator to resolve handlers.
	/// When unit testing, you can use the Register/Unregister methods to create callbacks.
	/// </summary>
	public static class DomainEvent
	{
		static DomainEvent()
		{
			CallbackStore = new LocalStore();
		}

		/// <summary>
		/// Gets or sets the callback store.
		/// </summary>
		/// <value>The callback store.</value>
		public static ICallbackStore CallbackStore { get; set; }

		/// <summary>
		/// Registers the callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		public static IDisposable RegisterCallback<TEvent>(Action<TEvent> callback)
			where TEvent : IDomainEvent
		{
			CallbackStore.Add(callback);
			return new DomainEventCallbackRemover(() => CallbackStore.Remove(callback));
		}

		/// <summary>
		/// Raises a domain event.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="event">The domain event.</param>
		public static void Raise<TEvent>(TEvent @event)
			where TEvent : IDomainEvent
		{
			if (ServiceLocator.Current != null)
			{
				var handlers = ServiceLocator.Current.GetAllInstances<IHandle<TEvent>>();
				if (handlers != null)
				{
					foreach (var handler in handlers)
					{
						handler.Handle(@event);
					}
				}
			}

			if (CallbackStore != null && CallbackStore.Callbacks != null)
			{
				foreach (var handler in CallbackStore.Callbacks)
				{
					if (handler is Action<TEvent>)
						((Action<TEvent>) handler)(@event);
				}
			}
		}

		/// <summary>
		/// Unregisters the callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		public static void UnregisterCallback<TEvent>(Action<TEvent> callback)
			where TEvent : IDomainEvent
		{
			CallbackStore.Remove(callback);
		}

		/// <summary>
		/// Clears all registered callbacks.
		/// </summary>
		public static void ClearCallbacks()
		{
			CallbackStore.Clear();
		}

		/// <summary>
		/// Used for removing a registered domain event callback
		/// </summary>
		private sealed class DomainEventCallbackRemover : IDisposable
		{
			private readonly Action dispose;

			public DomainEventCallbackRemover(Action disposeCallback)
			{
				this.dispose = disposeCallback;
			}

			public void Dispose()
			{
				dispose();
			}
		}
	}
}