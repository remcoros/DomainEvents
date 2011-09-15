namespace RawSoft.DomainEvents.Tests
{
	using System.Linq;
	using NUnit.Framework;
	using Rhino.Mocks;

	public class SimpleHandlerLocatorFixture
	{
		[SetUp] 
		public void SetUp()
		{
			
		}

		[Test]
		public void Can_get_correct_handlers_from_locator()
		{
			var h1_1 = GenerateHandlerFor<Event1>();
			var h1_2 = GenerateHandlerFor<Event1>();
			var h2 = GenerateHandlerFor<Event2>();

			var registry = new SimpleHandlerLocator();
			var locator = registry as IHandlerLocator;

			registry.Register(h1_1);
			registry.Register(h1_2);
			registry.Register(h2);


			Assert.That(locator.GetHandlersFor<Event1>().Count(), Is.EqualTo(2));
			Assert.That(locator.GetHandlersFor<Event2>().Count(), Is.EqualTo(1));
		}

		private IHandle<T> GenerateHandlerFor<T>() where T : IDomainEvent
		{
			return MockRepository.GenerateStub<IHandle<T>>();
		}
	}

	public class Event1 : IDomainEvent
	{
	}

	public class Event2 : IDomainEvent
	{
	}
}