namespace RawSoft.DomainEvents
{
	using System;

	public static class DomainEvent
	{
		static DomainEvent()
		{
			HandlerResolver = new SimpleResolver();
			HandlerStore = new ThreadStaticStore();
		}

		public static IHandlerResolver HandlerResolver { get; set; }

		public static IHandlerStore HandlerStore { get; set; }

		public static void Register<TEvent>(Action<TEvent> handler)
			where TEvent : IDomainEvent
		{
			HandlerStore.Add(handler);
		}

		public static void Raise<TEvent>(TEvent args)
			where TEvent : IDomainEvent
		{
			if (HandlerResolver != null)
			{
				var handlers = HandlerResolver.ResolveAll<TEvent>();
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
				foreach (Delegate handler in HandlerStore.Handlers)
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