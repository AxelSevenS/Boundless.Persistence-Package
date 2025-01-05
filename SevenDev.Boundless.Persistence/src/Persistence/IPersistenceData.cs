namespace SevenDev.Boundless.Persistence;

public interface IPersistenceData {
	object? Load();
}

public interface IPersistenceData<out T> : IPersistenceData where T : class {
	object? IPersistenceData.Load() => Load();
	public new T? Load();
}

public abstract class PersistenceData<T>(T item) : IPersistenceData<T> where T : class {
	public CustomizationData? Customization = item is ICustomizable customizable ? CustomizationData.GetFrom(customizable) : null;

	public T? Load() {
		T? instance = Instantiate();

		if (instance is not null) {
			if (instance is ICustomizable customizable && Customization is not null) {
				Customization.ApplyTo(customizable);
			}
			LoadInternal(instance);
		}

		return instance;
	}

	protected abstract T? Instantiate();
	protected virtual void LoadInternal(T item) { }
}

public class ItemPersistenceData<T>(T item) : PersistenceData<T>(item) where T : class, IItem<T> {
	private readonly ItemKey? DataKey = item.KeyProvider.ItemKey;

	public IItemData<T>? Data => _data ??=
		DataKey is null ? null
		: IItemData<T>.GetData(DataKey) ?? throw new System.InvalidOperationException($"Could not find Data of type {typeof(T)} with key {DataKey.String}.");
	private IItemData<T>? _data;

	protected sealed override T? Instantiate() => Data?.Instantiate();
}