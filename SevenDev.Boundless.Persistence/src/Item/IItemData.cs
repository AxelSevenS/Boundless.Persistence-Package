namespace SevenDev.Boundless.Persistence;

public interface IItemData : IUIObject {
	public IItemKeyProvider KeyProvider { get; }

	public bool Register(IItemDataRegistry registry);
	public object? Instantiate();
}

public interface IItemData<out T> : IItemData where T : IItem {
	bool IItemData.Register(IItemDataRegistry registry) => Register(registry);
	public new bool Register(IItemDataRegistry registry) => registry.RegisterData(this);

	object? IItemData.Instantiate() => Instantiate();
	public new T Instantiate();
}