namespace SevenDev.Boundless.Persistence;

public interface IItemDataProvider {
	IItemData<T>? GetData<T>(ItemKey key) where T : IItem;
}