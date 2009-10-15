namespace RawSoft.DomainEvents.Tests
{
	using System;
	using Microsoft.Practices.ServiceLocation;
	using NUnit.Framework;
	using Rhino.Mocks;

	[TestFixture]
	public class DomainEventFixture
	{
		[Test]
		public void RaiseEventShouldCallAllRegisteredHandlers()
		{
			bool called = false;
			Action<TestEvent> handler = x => called = true;

			DomainEvent.Register(handler);
			DomainEvent.Raise(new TestEvent());

			Assert.That(called, Is.EqualTo(true));
		}

		[Test]
		public void RaiseEventShouldCallHandlersFromContainer()
		{
			var container = MockRepository.GenerateStub<IServiceLocator>();
			ServiceLocator.SetLocatorProvider(() => container);
			var handler = MockRepository.GenerateStub<Handles<TestEvent>>();

			container.Stub(x => x.GetAllInstances<Handles<TestEvent>>())
				.Return(new[] {handler});

			var @event = new TestEvent();
			DomainEvent.Raise(@event);

			handler.AssertWasCalled(x => x.Handle(@event));
		}

		[Test]
		public void RegisterHandlerAddsHandlerToStore()
		{
			Action<TestEvent> handler = x => { };

			DomainEvent.Register(handler);

			Assert.That(DomainEvent.HandlerStore.Handlers, Has.Member(handler));
		}

		[Test]
		public void UnRegisterHandlerShouldRemoveHandlerFromStore()
		{
			Action<TestEvent> handler = x => { };

			DomainEvent.Register(handler);

			Assert.That(DomainEvent.HandlerStore.Handlers, Has.Member(handler));

			DomainEvent.UnregisterHandler(handler);

			Assert.That(DomainEvent.HandlerStore.Handlers, Has.No.Member(handler));
		}
	}

	public class TestEvent : IDomainEvent
	{
	}
}