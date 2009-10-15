namespace RawSoft.DomainEvents
{
	using System.Collections.Generic;

	public interface IHandlerResolver
	{
		IEnumerable<Handles<TEvent>> ResolveAll<TEvent>() where TEvent : IDomainEvent;
	}
}