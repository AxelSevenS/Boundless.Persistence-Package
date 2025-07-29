namespace Seven.Boundless.Persistence;

using System;

[Serializable]
public partial struct ItemKey : IEquatable<ItemKey> {
	[System.Text.RegularExpressions.GeneratedRegex(@"[^a-z0-9\-_=\|!#~]")]
	private static partial System.Text.RegularExpressions.Regex ItemKeySanitizeRegex();

	public static string ToItemKeyFormat(string key) {
		return ItemKeySanitizeRegex().Replace(key.ToLowerInvariant(), "_");
	}

	public readonly string String;

	public ItemKey(string @string) {
		if (string.IsNullOrWhiteSpace(@string)) throw new ArgumentException("Key cannot be null or empty.", nameof(@string));
		String = ToItemKeyFormat(@string);
	}

	public override string ToString() => String;
	public override int GetHashCode() => String.GetHashCode();
	public override bool Equals(object? obj) => obj is ItemKey key && String == key.String;

	public bool Equals(ItemKey other) => String == other.String;


	public static bool operator ==(ItemKey? left, ItemKey? right) => left?.Equals(right) ?? right is null;
	public static bool operator !=(ItemKey? left, ItemKey? right) => !(left == right);

	public static implicit operator string(ItemKey key) => key.String;
}