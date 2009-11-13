namespace RawSoft.DomainEvents.Tests
{
	using System;
	using Microsoft.Practices.ServiceLocation;
	using NUnit.Framework;
	using Rhino.Mocks;

	[TestFixture]
	public class DomainEventFixture
	{
		[SetUp]
		public void SetUp()
		{
			ServiceLocator.SetLocatorProvider(() => null);
		}

		[Test]
		public void test()
		{
			Assert.That(ServiceLocator.Current, Is.Null);
		}

		[Test]
		public void RaiseEventShouldCallAllRegisteredCallbacks()
		{
			bool called = false;
			Action<TestEvent> handler = x => called = true;

			DomainEvent.RegisterCallback(handler);
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
		public void RegisterCallbackAddsCallbackToStore()
		{
			Action<TestEvent> handler = x => { };

			DomainEvent.RegisterCallback(handler);

			Assert.That(DomainEvent.CallbackStore.Callbacks, Has.Member(handler));
		}

		[Test]
		public void UnRegisterCallbackShouldRemoveCallbackFromStore()
		{
			Action<TestEvent> handler = x => { };

			DomainEvent.RegisterCallback(handler);

			Assert.That(DomainEvent.CallbackStore.Callbacks, Has.Member(handler));

			DomainEvent.UnregisterCallback(handler);

			Assert.That(DomainEvent.CallbackStore.Callbacks, Has.No.Member(handler));
		}

		[Test]
		public void ClearCallbacksShouldRemoveAllCallbacksFromStore()
		{
			Action<TestEvent> handler = x => { };

			DomainEvent.RegisterCallback(handler);

			Assert.That(DomainEvent.CallbackStore.Callbacks, Has.Member(handler));

			DomainEvent.ClearCallbacks();

			Assert.That(DomainEvent.CallbackStore.Callbacks, Is.Empty);
		}
	}

	public class TestEvent : IDomainEvent
	{
	}
}