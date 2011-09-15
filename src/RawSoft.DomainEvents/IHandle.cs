namespace RawSoft.DomainEvents
{
	/// <summary>
	/// Base interface for domain event handlers
	/// </summary>
	/// <typeparam name="TEvent">The type of the event.</typeparam>
	public interface IHandle<in TEvent> where TEvent : IDomainEvent
	{
		/// <summary>
		/// Handles the specified @event.
		/// </summary>
		/// <param name="event">The @event.</param>
		void Handle(TEvent @event);
	}
}