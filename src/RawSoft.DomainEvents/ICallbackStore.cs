namespace RawSoft.DomainEvents
{
	using System;
	using System.Collections.Generic;

	public interface ICallbackStore
	{
		IEnumerable<Delegate> Handlers { get; }
		void Add<TEvent>(Action<TEvent> handler);

		void Remove<TEvent>(Action<TEvent> handler);

		void Clear();
	}
}