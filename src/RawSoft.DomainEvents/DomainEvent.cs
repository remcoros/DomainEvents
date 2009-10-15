namespace RawSoft.DomainEvents
{
	using System;
	using Microsoft.Practices.ServiceLocation;

	public static class DomainEvent
	{
		static DomainEvent()
		{
			CallbackStore = new ThreadStaticStore();
		}

		public static ICallbackStore CallbackStore { get; set; }

		public static void RegisterCallback<TEvent>(Action<TEvent> handler)
			where TEvent : IDomainEvent
		{
			CallbackStore.Add(handler);
		}

		public static void Raise<TEvent>(TEvent args)
			where TEvent : IDomainEvent
		{
			IServiceLocator locator = null;
			try
			{
				locator = ServiceLocator.Current;
			}
			catch (NullReferenceException)
			{
			}

			if (locator != null)
			{
				var handlers = locator.GetAllInstances<Handles<TEvent>>();
				if (handlers != null)
				{
					foreach (var handler in handlers)
					{
						handler.Handle(args);
					}
				}
			}

			if (CallbackStore != null && CallbackStore.Handlers != null)
			{
				foreach (var handler in CallbackStore.Handlers)
				{
					if (handler is Action<TEvent>)
						((Action<TEvent>) handler)(args);
				}
			}
		}

		public static void UnregisterCallback<TEvent>(Action<TEvent> handler)
			where TEvent : IDomainEvent
		{
			CallbackStore.Remove(handler);
		}

		public static void ClearCallbacks()
		{
			CallbackStore.Clear();
		}
	}
}