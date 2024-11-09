namespace SevenDev.Boundless.Persistence;

using Godot;

public interface ICustomization {
	Control? Build();

	ICustomizationState State { get; set; }
}