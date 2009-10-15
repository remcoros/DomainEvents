namespace RawSoft.DomainEvents
{
	public interface IHandles
	{
	}

	public abstract class Handles<TEvent> : IHandles
		where TEvent : IDomainEvent
	{
		public abstract void Handle(TEvent args);
	}
}