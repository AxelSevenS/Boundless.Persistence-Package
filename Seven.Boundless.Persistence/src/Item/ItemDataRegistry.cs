using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Seven.Boundless.Persistence;

public class ItemDataRegistry : IItemDataContainer {
	public readonly Action<string>? Logger;
	private readonly Dictionary<ItemKey, IItemData> _registry = [];


	public ItemDataRegistry(Action<string>? logger = null) {
		Logger = logger;
	}


	public IItemData<T>? GetData<T>(ItemKey key) where T : IItem {
		IItemData? untypedItem = _registry.TryGetValue(key, out IItemData? data) ? data : null;
		return untypedItem as IItemData<T>;
	}

	public bool RegisterData(IItemData data, bool overwrite = false) {
		ItemKey? key = data.ItemKey;
		if (!key.HasValue) {
			Logger?.Invoke($"Data key is null. {data}");
			return false;
		}

		ref IItemData? existingData = ref CollectionsMarshal.GetValueRefOrAddDefault(_registry, key.Value, out bool exists);

		if (!overwrite && exists) {
			Logger?.Invoke($"Data with key {key.Value} already exists.");
			return false;
		}

		existingData = data;
		Logger?.Invoke($"Registered {key.Value} => {data}");
		return true;
	}

	public bool UnregisterData(IItemData data) {
		ItemKey? key = data.ItemKey;
		if (!key.HasValue) {
			Logger?.Invoke($"Data key is null. {data}");
			return false;
		}

		_registry.Remove(key.Value);
		Logger?.Invoke($"Unregistered {key.Value} => {data}");
		return true;
	}

	public bool Clear() {
		_registry.Clear();
		Logger?.Invoke("Cleared all data.");
		return true;
	}
}