namespace RawSoft.DomainEvents.Tests
{
	using System.Linq;
	using NUnit.Framework;

	[TestFixture]
	public class SimpleResolverFixture
	{
		[Test]
		public void CanClearHandlers()
		{
			var resolver = new SimpleResolver();
			resolver.Register<TestEventHandler>();
			resolver.Register<TestEventHandler>();
			resolver.Register<TestEventHandler>();

			resolver.ClearAll();

			Assert.That(resolver.ResolveAll<TestEvent>(), Is.Null);
		}

		[Test]
		public void CanRegisterHandler()
		{
			var resolver = new SimpleResolver();
			resolver.Register<TestEventHandler>();

			Assert.That(resolver.ResolveAll<TestEvent>().Count(), Is.EqualTo(1));
		}

		[Test]
		public void CanRegisterMultipleHandlers()
		{
			var resolver = new SimpleResolver();
			resolver.Register<TestEventHandler>();
			resolver.Register<TestEventHandler>();
			resolver.Register<TestEventHandler>();

			Assert.That(resolver.ResolveAll<TestEvent>().Count(), Is.EqualTo(3));
		}

		[Test]
		public void ResolveWithoutHandlersReturnsNull()
		{
			var resolver = new SimpleResolver();
			Assert.That(resolver.ResolveAll<TestEvent>(), Is.Null);
		}
	}

	public class TestEventHandler : Handles<TestEvent>
	{
		public override void Handle(TestEvent args)
		{
		}
	}
}