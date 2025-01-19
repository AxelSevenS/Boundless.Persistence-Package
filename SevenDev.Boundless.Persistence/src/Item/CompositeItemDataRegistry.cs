namespace SevenDev.Boundless.Persistence;

using System.Collections.Generic;
using System.Linq;

public sealed class CompositeItemDataRegistry : IItemDataProvider {
	private List<IItemDataProvider> _registries = [];


	public CompositeItemDataRegistry(params IItemDataProvider[] registries) {
		_registries.AddRange(registries);
	}


	public void AddRegistry(IItemDataProvider registry) {
		_registries.Add(registry);
		_registries = [.._registries.Distinct()];
	}
	public void RemoveRegistry(IItemDataProvider registry) {
		_registries.Remove(registry);
	}

	public IItemData<T>? GetData<T>(ItemKey key) where T : IItem {
		foreach (IItemDataProvider registry in _registries) {
			IItemData<T>? data = registry.GetData<T>(key);
			if (data != null) {
				return data;
			}
		}
		return null;
	}
}