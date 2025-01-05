namespace SevenDev.Boundless.Persistence;

using System;
using Godot;

[Serializable]
public class ItemKey : IEquatable<ItemKey>, IDisposable {
	private bool _disposed = false;

	public string String => _string;
	private readonly string _string;

	public ItemKey(string @string) {
		if (string.IsNullOrWhiteSpace(@string)) throw new ArgumentException("Key cannot be null or empty.", nameof(@string));
		_string = @string.ToSnakeCase();
	}

	~ItemKey() => Dispose(false);
	public void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing) {
		if (_disposed) return;
		_disposed = true;
	}

	public override string ToString() => String;
	public override int GetHashCode() => String.GetHashCode();
	public override bool Equals(object? obj) => obj is ItemKey key && String == key.String;

	public bool Equals(ItemKey? other) => other is not null && String == other.String;


	public static bool operator ==(ItemKey? left, ItemKey? right) => left?.Equals(right) ?? right is null;
	public static bool operator !=(ItemKey? left, ItemKey? right) => !(left == right);
}