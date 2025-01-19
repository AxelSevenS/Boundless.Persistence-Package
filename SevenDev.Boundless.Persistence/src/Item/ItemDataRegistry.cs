using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SevenDev.Boundless.Persistence;

public class ItemDataRegistry : IItemDataRegistry {
	public readonly Action<string>? Logger;
	private readonly Dictionary<ItemKey, IItemData> _registry = [];


	public ItemDataRegistry(Action<string>? logger = null) {
		Logger = logger;
	}


	public IItemData<T>? GetData<T>(ItemKey key) where T : IItem {
		IItemData? untypedItem = _registry.TryGetValue(key, out IItemData? data) ? data : null;
		return untypedItem as IItemData<T>;
	}

	public bool RegisterData<T>(IItemData<T> data, bool overwrite = false) where T : IItem {
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			Logger?.Invoke($"Data key is null. {data} (type {typeof(T)})");
			return false;
		}

		ref IItemData? existingData = ref CollectionsMarshal.GetValueRefOrAddDefault(_registry, key, out bool exists);

		if (!overwrite && exists) {
			Logger?.Invoke($"Data with key {data.KeyProvider.ItemKey} (type {typeof(T)}) already exists.");
			return false;
		}

		existingData = data;
		Logger?.Invoke($"Registered {key} => {data} (type {typeof(T)})");
		return true;
	}

	public bool UnregisterData<T>(IItemData<T> data) where T : IItem {
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			Logger?.Invoke($"Data key is null. {data} (type {typeof(T)})");
			return false;
		}

		_registry.Remove(key);
		Logger?.Invoke($"Unregistered {key} => {data} (type {typeof(T)})");
		return true;
	}
}