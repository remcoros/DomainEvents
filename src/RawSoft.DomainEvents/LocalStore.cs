namespace RawSoft.DomainEvents
{
	using System;
	using System.Collections.Generic;
	using System.Web;

	/// <summary>
	/// A local store for callbacks
	/// </summary>
	public class LocalStore : ICallbackStore
	{
		private static readonly string StoreKey = typeof(LocalStore).FullName + "_callbacks";

		[ThreadStatic] static IList<Delegate> callbacks;

		protected IList<Delegate> CallbacksInternal
		{
			get
			{
				if (HttpContext.Current != null)
				{
					var lst = HttpContext.Current.Items[StoreKey] as IList<Delegate>;
					if (lst == null)
					{
						lst = new List<Delegate>();
						HttpContext.Current.Items[StoreKey] = lst;
					}

					return lst;
				}

				if (callbacks == null)
				{
					callbacks = new List<Delegate>();
				}

				return callbacks;
			}
		}

		#region ICallbackStore Members

		/// <summary>
		/// Gets the callbacks in this store.
		/// </summary>
		/// <value>The callbacks.</value>
		public IEnumerable<Delegate> Callbacks
		{
			get { return CallbacksInternal; }
		}

		/// <summary>
		/// Clear all callbacks from this store.
		/// </summary>
		public void Clear()
		{
			CallbacksInternal.Clear();
		}

		/// <summary>
		/// Adds the specified callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		public void Add<TEvent>(Action<TEvent> callback)
		{
			CallbacksInternal.Add(callback);
		}

		/// <summary>
		/// Removes the specified callback.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="callback">The callback.</param>
		public void Remove<TEvent>(Action<TEvent> callback)
		{
			CallbacksInternal.Remove(callback);
		}

		#endregion
	}
}