namespace SevenDev.Boundless.Persistence;

public interface IItemKeyProvider {
	ItemKey? ItemKey { get; }
	public IItemData? GetData() => IItemData.GetData(this);
}

public interface IItemKeyProvider<out T> : IItemKeyProvider where T : IItem<T> {
	public IItemData<T>? GetData() => IItemData<T>.GetData(this);
}