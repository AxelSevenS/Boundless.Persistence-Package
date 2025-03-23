namespace SevenDev.Boundless.Persistence;

public interface IItemData : IUIObject {
	public ItemKey? ItemKey { get; }
	public object? Instantiate();
}

public interface IItemData<out T> : IItemData where T : IItem {
	object? IItemData.Instantiate() => Instantiate();
	public new T Instantiate();
}