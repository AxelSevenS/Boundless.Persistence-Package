namespace SevenDev.Boundless.Persistence;

public interface IItemData : IUIObject {
	public IItemKeyProvider KeyProvider { get; }

	public bool Register(ItemDataRegistry registry);
	public object? Instantiate();
}

public interface IItemData<out T> : IItemData where T : IItem {
	bool IItemData.Register(ItemDataRegistry registry) => Register(registry);
	public new bool Register(ItemDataRegistry registry) => registry.RegisterData(this);

	object? IItemData.Instantiate() => Instantiate();
	public new T Instantiate();
}