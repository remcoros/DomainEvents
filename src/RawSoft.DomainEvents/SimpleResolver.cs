namespace RawSoft.DomainEvents
{
	using System;
	using System.Collections.Generic;

	public class SimpleResolver : IHandlerResolver
	{
		IDictionary<Type, IList<Type>> handlers = new Dictionary<Type, IList<Type>>();

		#region IHandlerResolver Members

		public IEnumerable<Handles<TEvent>> ResolveAll<TEvent>()
			where TEvent : IDomainEvent
		{
			if (!handlers.ContainsKey(typeof (TEvent)))
				return null;

			IList<Type> types = handlers[typeof (TEvent)];
			var result = new Handles<TEvent>[types.Count];
			for (int i = 0; i < types.Count; i++)
			{
				result[i] = (Handles<TEvent>) Activator.CreateInstance(types[i]);
			}

			return result;
		}

		#endregion

		public void Register<THandler>()
			where THandler : IHandles
		{
			var type = typeof (THandler);
			var baseType = type;
			while (baseType != typeof (object))
			{
				if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof (Handles<>))
				{
					break;
				}

				baseType = baseType.BaseType;
			}

			if (baseType == null)
			{
				throw new ArgumentException("Type does not inherit from Handles`1");
			}

			var argType = baseType.GetGenericArguments()[0];

			if (!handlers.ContainsKey(argType))
			{
				handlers[argType] = new List<Type>();
			}

			handlers[argType].Add(type);
		}

		public void ClearAll()
		{
			handlers = new Dictionary<Type, IList<Type>>();
		}
	}
}