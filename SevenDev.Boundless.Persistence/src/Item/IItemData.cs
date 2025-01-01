namespace SevenDev.Boundless.Persistence;

using System.Collections.Generic;
using Godot;

public interface IItemData : IUIObject {
	protected static readonly Dictionary<string, IItemData> Registry = [];
	public IDataKeyProvider KeyProvider { get; }

	public static IItemData? GetData(string key) => Registry.TryGetValue(key, out IItemData? data) ? data : null;
	public static IItemData? GetData(IDataKeyProvider? keyProvider) => keyProvider is not null ? GetData(keyProvider.Key) : null;
	protected static bool RegisterData(IItemData data, bool log, bool overwrite = false) {
		string key = data.KeyProvider.Key;
		if (string.IsNullOrWhiteSpace(key)) {
			if (log) GD.PrintErr("Could not register: Data key is null or empty.");
			return false;
		}

		if (!overwrite && Registry.ContainsKey(key)) {
			if (log) GD.PrintErr($"Data with key {key} already exists.");
			return false;
		}

		Registry[key] = data;
		if (log) GD.Print($"Registered {key} => {data}");
		return true;
	}
	public static bool RegisterData(IItemData data, bool overwrite = false) => RegisterData(data, true, overwrite);

	public static bool UnregisterData(IItemData data, bool log) {
		string key = data.KeyProvider.Key;
		if (string.IsNullOrWhiteSpace(key)) {
			if (log) GD.PrintErr("Could not unregister: Data key is null or empty.");
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
	private static readonly Dictionary<string, IItemData<T>> TypedRegistry = [];

	IDataKeyProvider IItemData.KeyProvider => KeyProvider;
	public new IDataKeyProvider<T> KeyProvider { get; }


	public static new IItemData<T>? GetData(string key) => TypedRegistry.TryGetValue(key, out IItemData<T>? data) ? data : null;
	public static new IItemData<T>? GetData(IDataKeyProvider<T>? keyProvider) => keyProvider is not null ? GetData(keyProvider.Key) : null;
	protected static bool RegisterData(IItemData<T> data, bool log, bool overwrite = false) {
		if (!IItemData.RegisterData(data, false, overwrite)) return false;

		if (TypedRegistry.ContainsKey(data.KeyProvider.Key) && !overwrite) {
			if (log) GD.PrintErr($"Data with key {data.KeyProvider.Key} (type {typeof(T)}) already exists.");
			return false;
		}

		string key = data.KeyProvider.Key;
		TypedRegistry[key] = data;
		if (log) GD.Print($"Registered {key} => {data} (type {typeof(T)})");
		return true;
	}
	public static bool RegisterData(IItemData<T> data, bool overwrite = false) => RegisterData(data, true, overwrite);

	protected static bool UnregisterData(IItemData<T> data, bool log) {
		if (!IItemData.UnregisterData(data, false)) return false;

		string key = data.KeyProvider.Key;
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