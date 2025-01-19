using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SevenDev.Boundless.Persistence;

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
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			Logger?.Invoke($"Data key is null. {data}");
			return false;
		}

		ref IItemData? existingData = ref CollectionsMarshal.GetValueRefOrAddDefault(_registry, key, out bool exists);

		if (!overwrite && exists) {
			Logger?.Invoke($"Data with key {data.KeyProvider.ItemKey} already exists.");
			return false;
		}

		existingData = data;
		Logger?.Invoke($"Registered {key} => {data}");
		return true;
	}

	public bool UnregisterData(IItemData data) {
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			Logger?.Invoke($"Data key is null. {data}");
			return false;
		}

		_registry.Remove(key);
		Logger?.Invoke($"Unregistered {key} => {data}");
		return true;
	}

	public bool Clear() {
		_registry.Clear();
		Logger?.Invoke("Cleared all data.");
		return true;
	}
}