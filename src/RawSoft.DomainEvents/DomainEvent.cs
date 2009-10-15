namespace RawSoft.DomainEvents
{
	using System;
	using Microsoft.Practices.ServiceLocation;

	public static class DomainEvent
	{
		static DomainEvent()
		{
			HandlerStore = new ThreadStaticStore();
		}

		public static IHandlerStore HandlerStore { get; set; }

		public static void Register<TEvent>(Action<TEvent> handler)
			where TEvent : IDomainEvent
		{
			HandlerStore.Add(handler);
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

			if (HandlerStore != null && HandlerStore.Handlers != null)
			{
				foreach (var handler in HandlerStore.Handlers)
				{
					if (handler is Action<TEvent>)
						((Action<TEvent>) handler)(args);
				}
			}
		}

		public static void UnregisterHandler<TEvent>(Action<TEvent> handler)
			where TEvent : IDomainEvent
		{
			HandlerStore.Remove(handler);
		}

		public static void ClearHandlers()
		{
			HandlerStore.Clear();
		}
	}
}