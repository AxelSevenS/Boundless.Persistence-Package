namespace SevenDev.Boundless.Persistence;

using System.Collections.Generic;

public interface ICustomizable : IUIObject {
	Dictionary<string, ICustomization> GetCustomizations() => [];
}