using System;
using System.Collections.Generic;
using System.Dynamic;

namespace SirenMapper
{
    public abstract class DynamicJsonClass : DynamicObject
	{
		public class IfArrayValue
		{
			public Func<bool> If;
			public Func<object> Value;
		}

		private readonly Dictionary<string, object> items = new Dictionary<string, object>();
		public readonly Dictionary<string, IfArrayValue> itemsArrays = new Dictionary<string, IfArrayValue>();

		protected void AddItemIfAny(string property, Func<Boolean> ifValue, Func<object> value)
		{
			this.itemsArrays.Add(property, new IfArrayValue { If = ifValue, Value = value});
		}

		protected T GetDictionaryOrDefault<T>(string name)
		{
			if (items.ContainsKey(name))
			{
				return (T) items[name];
			}

			return default(T);
		}

		protected void SetDictionary<T>(string name, T value)
		{
			items[name] = value;
		}

		protected void Remove(string name)
		{
			if (items.ContainsKey(name))
			{
				this.items.Remove(name);
			}
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			foreach (var key in items.Keys)
			{
				yield return key;
			}

			foreach (var i in itemsArrays)
			{
				if (i.Value.If())
				{
					yield return i.Key;
				}
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (itemsArrays.ContainsKey(binder.Name))
			{
				result = itemsArrays[binder.Name].Value();
				return true;
			}

			if (items.ContainsKey(binder.Name))
			{
				result = items[binder.Name];
				return true;
			}

			result = null;
			return false;
			//return base.TryGetMember(binder, out result);
		}
	}
}