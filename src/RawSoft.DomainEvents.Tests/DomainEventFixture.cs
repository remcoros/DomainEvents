namespace RawSoft.DomainEvents.Tests
{
	using System;
	using NUnit.Framework;
	using Rhino.Mocks;

	[TestFixture]
	public class DomainEventFixture
	{
		[SetUp]
		public void SetUp()
		{
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
			var container = MockRepository.GenerateStub<IHandlerLocator>();
			DomainEvent.HandlerLocator = container;
			var handler = MockRepository.GenerateStub<IHandle<TestEvent>>();

			container.Stub(x => x.GetHandlersFor<TestEvent>())
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

		[Test]
		public void Can_use_disposable_pattern()
		{
			Action<TestEvent> handler = x => { };

			using (DomainEvent.RegisterCallback(handler))
			{
				Assert.That(DomainEvent.CallbackStore.Callbacks, Has.Member(handler));
			}

			Assert.That(DomainEvent.CallbackStore.Callbacks, Has.No.Member(handler));			
		}
	}

	public class TestEvent : IDomainEvent
	{
	}
}