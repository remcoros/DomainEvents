namespace RawSoft.DomainEvents
{
	public interface IHandle<TEvent> where TEvent : IDomainEvent
	{
		void Handle(TEvent @event);
	}
}