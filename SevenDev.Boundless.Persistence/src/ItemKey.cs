namespace SevenDev.Boundless.Persistence;

using System;
using Godot;

[Serializable]
public struct ItemKey : IEquatable<ItemKey> {
	public readonly string String;

	public ItemKey(string @string) {
		if (string.IsNullOrWhiteSpace(@string)) throw new ArgumentException("Key cannot be null or empty.", nameof(@string));
		String = @string.ToSnakeCase();
	}

	public override string ToString() => String;
	public override int GetHashCode() => String.GetHashCode();
	public override bool Equals(object? obj) => obj is ItemKey key && String == key.String;

	public bool Equals(ItemKey other) => String == other.String;


	public static bool operator ==(ItemKey? left, ItemKey? right) => left?.Equals(right) ?? right is null;
	public static bool operator !=(ItemKey? left, ItemKey? right) => !(left == right);

	public static implicit operator string(ItemKey key) => key.String;
}