/*
	This file is just a public test file. It is not part of any project... yet.

	This interval tree is inspired on RangeTree by mbuchetics on GitHub (https://github.com/mbuchetics/RangeTree).
*/
using System.Collections.Generic;


public class IntervalTree<TValue> {
	private IntervalTree.Node<TValue>? _root;
	private readonly List<RangePair<TValue>> _ranges = new List<RangePair<TValue>>();

	public IntervalTree.Node<TValue>? root => _root;



	public static readonly Comparer<RangePair<TValue>> _comparer =
		Comparer<RangePair<TValue>>.Create(
			(a, b) => a.from.CompareTo(b.from)
	);


	public IntervalTree() { }

	public IntervalTree(IEnumerable<RangePair<TValue>> ranges) {
		foreach (RangePair<TValue> range in ranges) {
			_ranges.Add(range);
		}

	}


	public void Add(float from, float to, TValue value) {
		RangePair<TValue> range = new RangePair<TValue>(from, to, value);

		_ranges.Add(range);
	}
	public void Add(RangePair<TValue> range) {
		Add(range.from, range.to, range.value);
	}
	public void Add(IEnumerable<RangePair<TValue>> ranges) {
		foreach (RangePair<TValue> range in ranges) {
			Add(range);
		}
	}


	public void Build() {
		if (_ranges.Count == 0) return;
		if (_root != null) _root = null;

		_root = new IntervalTree.Node<TValue>(_ranges);
	}

	public IEnumerable<RangePair<TValue>> Search(float key) {
		if (_root == null) yield break;

		foreach (RangePair<TValue> range in _root.Search(key)) {
			yield return range;
		}
	}
}

