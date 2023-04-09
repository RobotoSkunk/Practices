/*
	This file is just a public test file. It is not part of any project... yet.

	This interval tree is inspired on RangeTree by mbuchetics on GitHub (https://github.com/mbuchetics/RangeTree).
*/

namespace IntervalTree {
	public class Node<TValue> {
		private RangePair<TValue>? _range;
		private Node<TValue>? _left;
		private Node<TValue>? _right;

		public Node(RangePair<TValue> range) {
			_range = range;
		}
		public Node(float from, float to, TValue value) {
			_range = new RangePair<TValue>(from, to, value);
		}


		public Node(List<RangePair<TValue>> ranges) {
			if (ranges.Count == 0) return;

			_range = ranges[0];
			if (ranges.Count == 1) return;


			ranges.Sort(IntervalTree<TValue>._comparer);


			var mid = ranges[ranges.Count / 2];

			var left = ranges.GetRange(0, ranges.Count / 2);
			var right = ranges.GetRange(ranges.Count / 2 + 1, ranges.Count / 2 - 1);

			_range = mid;

			_left = new Node<TValue>(left);
			_right = new Node<TValue>(right);
		}
	}
}

