namespace RawSoft.DomainEvents
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Simple handler locator for DomainEvent (if you don't use an IoC container). This class is not thread safe!
	/// </summary>
	public class SimpleHandlerLocator : IHandlerLocator
	{
		private readonly IDictionary<Type, IList<object>> handlers = new Dictionary<Type, IList<object>>();

		public IEnumerable<IHandle<TEvent>> GetHandlersFor<TEvent>() where TEvent : IDomainEvent
		{
			var type = typeof (TEvent);

			if (handlers.ContainsKey(type))
				return handlers[type].OfType<IHandle<TEvent>>();

			return new IHandle<TEvent>[] {};
		}

		public void Register<T>(IHandle<T> handle) where T : IDomainEvent
		{
			var type = typeof (T);
			IList<object> thehandlers;
			if (!handlers.ContainsKey(type))
			{
				thehandlers = new List<object>();
				handlers[type] = thehandlers;
			}
			else
			{
				thehandlers = handlers[type];
			}
			
			thehandlers.Add(handle);
		}
	}
}