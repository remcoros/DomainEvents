namespace RawSoft.DomainEvents
{
	using System;
	using System.Collections.Generic;

	public class ThreadStaticStore : ICallbackStore
	{
		[ThreadStatic] static IList<Delegate> handlers;

		#region ICallbackStore Members

		public IEnumerable<Delegate> Handlers
		{
			get { return handlers; }
		}

		public void Clear()
		{
			handlers = null;
		}

		public void Add<TEvent>(Action<TEvent> handler)
		{
			if (handlers == null)
			{
				handlers = new List<Delegate>();
			}

			handlers.Add(handler);
		}

		public void Remove<TEvent>(Action<TEvent> handler)
		{
			handlers.Remove(handler);
		}

		#endregion
	}
}