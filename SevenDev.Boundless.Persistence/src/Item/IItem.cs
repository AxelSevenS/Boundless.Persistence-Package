namespace SevenDev.Boundless.Persistence;

public interface IItem {
	public IItemKeyProvider KeyProvider { get; }
}

public interface IItem<out T> : IItem where T : IItem<T> {
	IItemKeyProvider IItem.KeyProvider => KeyProvider;
	public new IItemKeyProvider<T> KeyProvider { get; }
}