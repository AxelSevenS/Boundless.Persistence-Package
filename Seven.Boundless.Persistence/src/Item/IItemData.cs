namespace Seven.Boundless.Persistence;

public interface IItemData {
	public ItemKey? ItemKey { get; }
	public object? Instantiate();
}

public interface IItemData<out T> : IItemData where T : IItem {
	object? IItemData.Instantiate() => Instantiate();
	public new T Instantiate();
}