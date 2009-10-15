namespace RawSoft.DomainEvents
{
	using System.Collections.Generic;
	using Microsoft.Practices.ServiceLocation;

	public class ServiceLocatorHandlerResolver : IHandlerResolver
	{
		#region IHandlerResolver Members

		public IEnumerable<Handles<TEvent>> ResolveAll<TEvent>() where TEvent : IDomainEvent
		{
			if (ServiceLocator.Current == null)
			{
				return null;
			}

			return ServiceLocator.Current.GetAllInstances<Handles<TEvent>>();
		}

		#endregion
	}
}