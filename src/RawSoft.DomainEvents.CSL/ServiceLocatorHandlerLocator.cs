namespace RawSoft.DomainEvents.CSL
{
	using System.Collections.Generic;
	using Microsoft.Practices.ServiceLocation;

	/// <summary>
	/// IHandlerLocator implementation for Microsoft Common Service Locator
	/// </summary>
	public class ServiceLocatorHandlerLocator : IHandlerLocator
	{
		/// <summary>
		/// Gets all the handlers for TEvent event.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <returns>all the handlers for TEvent event</returns>
		public IEnumerable<IHandle<TEvent>> GetHandlersFor<TEvent>() where TEvent : IDomainEvent
		{
			return ServiceLocator.Current.GetAllInstances<IHandle<TEvent>>();
		}
	}
}
