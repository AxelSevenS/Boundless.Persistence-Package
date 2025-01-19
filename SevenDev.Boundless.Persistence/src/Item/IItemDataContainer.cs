namespace SevenDev.Boundless.Persistence;

public interface IItemDataContainer : IItemDataProvider {
	bool RegisterData(IItemData data, bool overwrite = false);
	bool UnregisterData(IItemData data);

	bool Clear();
}