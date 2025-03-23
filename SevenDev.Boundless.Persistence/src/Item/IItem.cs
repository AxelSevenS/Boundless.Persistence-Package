namespace SevenDev.Boundless.Persistence;

public interface IItem {
	public IItemData? Data { get; }
}

public interface IItem<T> : IItem where T : IItem<T> {
	IItemData? IItem.Data => Data;
	public new IItemData<T>? Data { get; }
}