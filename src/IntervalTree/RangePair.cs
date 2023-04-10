/*
	This file is just a public test file. It is not part of any project... yet.

	This interval tree is inspired on RangeTree by mbuchetics on GitHub (https://github.com/mbuchetics/RangeTree).
*/
using System.Diagnostics.CodeAnalysis;


public struct RangePair<TValue> {
	public float from { get; }
	public float to { get; }
	public TValue value { get; }

	public float midpoint { get => (from + to) / 2f; }


	public RangePair(float from, float to, TValue value) {
		if (from > to) throw new System.ArgumentException("from must be less than or equal to to", nameof(from));

		this.from = from;
		this.to = to;
		this.value = value;
	}

	public bool Contains(float key) {
		return from <= key && key <= to;
	}

	public override string ToString() {
		return $"[{from}, {to}] {value}";
	}

	public override int GetHashCode() {
		int hash = from.GetHashCode() ^ to.GetHashCode();

		if (value != null) hash ^= value.GetHashCode();

		return hash;
	}

	public override bool Equals([NotNullWhen(true)] object? obj) {
		if (obj is RangePair<TValue> other) {
			return from == other.from && to == other.to
				&& EqualityComparer<TValue>.Default.Equals(value, other.value);
		}

		return false;
	}

	public static bool operator ==(RangePair<TValue> a, RangePair<TValue> b) => a.Equals(b);
	public static bool operator !=(RangePair<TValue> a, RangePair<TValue> b) => !(a == b);
}
