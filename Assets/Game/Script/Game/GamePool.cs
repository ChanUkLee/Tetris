using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePool
{
	public class PoolContainer<T>
	{
		private T item;
		
		public bool Used { get; private set; }
		
		public void Consume()
		{
			Used = true;
		}
		
		public T Item
		{
			get
			{
				return item;
			}
			set
			{
				item = value;
			}
		}
		
		public void Release()
		{
			Used = false;
		}
	}

	public class Pool<T>
	{
		private List<PoolContainer<T>> list;
		private Dictionary<T, PoolContainer<T>> lookup;
		private Func<T> factoryFunc;
		private int lastIndex = 0;

		public Pool(Func<T> factoryFunc, int initialSize)
		{
			this.factoryFunc = factoryFunc;

			list = new List<PoolContainer<T>>(initialSize);
			lookup = new Dictionary<T, PoolContainer<T>>(initialSize);

			Warm(initialSize);
		}

		private void Warm(int capacity)
		{
			for (int i = 0; i < capacity; i++)
			{
				CreateConatiner();
			}
		}

		private PoolContainer<T> CreateConatiner()
		{
			var container = new PoolContainer<T>();
			container.Item = factoryFunc();
			list.Add(container);
			return container;
		}

		public T GetItem(int i) {
			if (0 <= i && i < list.Count) {
				var container = list[i];
				return container.Item;
			}

			return default(T);
		}

		public T GetItem()
		{
			PoolContainer<T> container = null;
			for (int i = 0; i < list.Count; i++)
			{
				lastIndex++;
				if (lastIndex > list.Count - 1) lastIndex = 0;
				
				if (list[lastIndex].Used)
				{
					continue;
				}
				else
				{
					container = list[lastIndex];
					break;
				}
			}

			if (container == null)
			{
				//container = CreateConatiner();
				return default(T);
			}

			container.Consume();
			lookup.Add(container.Item, container);
			return container.Item;
		}

		public void ReleaseItem(object item)
		{
			ReleaseItem((T) item);
		}

		public void ReleaseItem(T item)
		{
			if (lookup.ContainsKey(item))
			{
				var container = lookup[item];
				container.Release();
				lookup.Remove(item);
			}
			else
			{
				GameDebug.Error ("This object pool does not contain the item provided: " + item);
			}
		}

		public int Count
		{
			get { return list.Count; }
		}

		public int CountUsedItems
		{
			get { return lookup.Count; }
		}
	}
}
