namespace SevenDev.Boundless.Persistence;

public interface IPersistenceData {
	object? Load(IItemDataRegistry registry);
}

public interface IPersistenceData<out T> : IPersistenceData where T : class {
	object? IPersistenceData.Load(IItemDataRegistry registry) => Load(registry);
	public new T? Load(IItemDataRegistry registry);
}

public abstract class PersistenceData<T>(T item) : IPersistenceData<T> where T : class {
	public CustomizationData? Customization = item is ICustomizable customizable ? CustomizationData.GetFrom(customizable) : null;

	public T? Load(IItemDataRegistry registry) {
		T? instance = Instantiate(registry);

		if (instance is not null) {
			if (instance is ICustomizable customizable && Customization is not null) {
				Customization.ApplyTo(customizable);
			}
			LoadInternal(instance, registry);
		}

		return instance;
	}

	protected abstract T? Instantiate(IItemDataRegistry registry);
	protected virtual void LoadInternal(T item, IItemDataRegistry registry) { }
}

public class ItemPersistenceData<T>(T item) : PersistenceData<T>(item) where T : class, IItem {
	private readonly ItemKey? DataKey = item.KeyProvider.ItemKey;

	private IItemData<T>? _data;

	protected sealed override T? Instantiate(IItemDataRegistry registry) {
		if (_data is null && DataKey is not null) {
			_data = registry.GetData<T>(DataKey)
				?? throw new System.InvalidOperationException($"Could not find Data of type {typeof(T)} with key {DataKey.String}.");
		}

		return _data?.Instantiate();
	}
}