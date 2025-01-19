namespace SevenDev.Boundless.Persistence;

public interface IItemDataRegistry {
	IItemData<T>? GetData<T>(ItemKey key) where T : IItem;
	bool RegisterData<T>(IItemData<T> data, bool overwrite = false) where T : IItem;
	bool UnregisterData<T>(IItemData<T> data) where T : IItem;
}