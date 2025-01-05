namespace SevenDev.Boundless.Persistence;

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;

public interface IItemData : IUIObject {
	protected static readonly Dictionary<ItemKey, IItemData> Registry = [];
	public IItemKeyProvider KeyProvider { get; }

	public static IItemData? GetData(ItemKey key) => Registry.TryGetValue(key, out IItemData? data) ? data : null;
	public static IItemData? GetData(IItemKeyProvider keyProvider) => keyProvider.ItemKey is not null ? GetData(keyProvider.ItemKey) : null;
	protected static bool RegisterData(IItemData data, bool log, bool overwrite = false) {
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			if (log) GD.PrintErr($"Data key is null. {data}");
			return false;
		}

		ref IItemData? existingData = ref CollectionsMarshal.GetValueRefOrAddDefault(Registry, key, out bool exists);

		if (!overwrite && exists) {
			if (log) GD.PrintErr($"Data with key {key} already exists.");
			return false;
		}

		existingData = data;
		if (log) GD.Print($"Registered {key} => {data}");
		return true;
	}
	public static bool RegisterData(IItemData data, bool overwrite = false) => RegisterData(data, true, overwrite);

	public static bool UnregisterData(IItemData data, bool log) {
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			if (log) GD.PrintErr($"Data key is null. {data}");
			return false;
		}

		Registry.Remove(key);
		if (log) GD.Print($"Unregistered {key} => {data}");
		return true;
	}
	public static bool UnregisterData(IItemData data) => UnregisterData(data, true);


	public void Register(bool force = false) => RegisterData(this, force);
	public void Unregister() => UnregisterData(this);

	public object? Instantiate();
}

public interface IItemData<out T> : IItemData where T : IItem<T> {
	private static readonly Dictionary<ItemKey, IItemData<T>> TypedRegistry = [];

	IItemKeyProvider IItemData.KeyProvider => KeyProvider;
	public new IItemKeyProvider<T> KeyProvider { get; }


	public static new IItemData<T>? GetData(ItemKey key) => TypedRegistry.TryGetValue(key, out IItemData<T>? data) ? data : null;
	public static new IItemData<T>? GetData(IItemKeyProvider<T> keyProvider) => keyProvider.ItemKey is not null ? GetData(keyProvider.ItemKey) : null;
	protected static bool RegisterData(IItemData<T> data, bool log, bool overwrite = false) {
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			if (log) GD.PrintErr($"Data key is null. {data} (type {typeof(T)})");
			return false;
		}

		if (!IItemData.RegisterData(data, false, overwrite)) return false;

		ref IItemData<T>? existingData = ref CollectionsMarshal.GetValueRefOrAddDefault(TypedRegistry, key, out bool exists);

		if (!overwrite && exists) {
			if (log) GD.PrintErr($"Data with key {data.KeyProvider.ItemKey} (type {typeof(T)}) already exists.");
			return false;
		}

		existingData = data;
		if (log) GD.Print($"Registered {key} => {data} (type {typeof(T)})");
		return true;
	}
	public static bool RegisterData(IItemData<T> data, bool overwrite = false) => RegisterData(data, true, overwrite);

	protected static bool UnregisterData(IItemData<T> data, bool log) {
		ItemKey? key = data.KeyProvider.ItemKey;
		if (key is null) {
			if (log) GD.PrintErr($"Data key is null. {data} (type {typeof(T)})");
			return false;
		}

		if (!IItemData.UnregisterData(data, false)) return false;

		TypedRegistry.Remove(key);
		if (log) GD.Print($"Unregistered {key} => {data} (type {typeof(T)})");
		return true;
	}
	public static bool UnregisterData(IItemData<T> data) => RegisterData(data, true);

	void IItemData.Register(bool force) => Register(force);
	public new sealed void Register(bool force = false) => RegisterData(this, force);

	void IItemData.Unregister() => Unregister();
	public new sealed void Unregister() => UnregisterData(this);

	object? IItemData.Instantiate() => Instantiate();
	public new T Instantiate();
}