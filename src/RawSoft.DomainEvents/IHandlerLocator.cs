namespace RawSoft.DomainEvents
{
	using System.Collections.Generic;

	/// <summary>
	/// Locates all handlers for an event type
	/// </summary>
	public interface IHandlerLocator
	{
		/// <summary>
		/// Gets all the handlers for TEvent event.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <returns>all the handlers for TEvent event</returns>
		IEnumerable<IHandle<TEvent>> GetHandlersFor<TEvent>() where TEvent : IDomainEvent;
	}
}