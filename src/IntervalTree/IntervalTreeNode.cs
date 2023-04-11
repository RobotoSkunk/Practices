/*
	This file is just a public test file. It is not part of any project... yet.

	This interval tree is inspired on RangeTree by mbuchetics on GitHub (https://github.com/mbuchetics/RangeTree).
*/



namespace IntervalTree {
	public class Node<TValue> {
		private RangePair<TValue> _range;
		private Node<TValue>? _left;
		private Node<TValue>? _right;
		private float _max;

		public float max => _max;
		public RangePair<TValue> range => _range;


		public Node(RangePair<TValue> range) {
			_range = range;
		}
		public Node(float from, float to, TValue value) {
			_range = new RangePair<TValue>(from, to, value);
		}


		public Node(RangePair<TValue>[] ranges) {
			if (ranges.Length == 0) return;

			_range = ranges[0];
			if (ranges.Length == 1) return;

			int midIndex = ranges.Length / 2;

			RangePair<TValue> mid = ranges[midIndex];
			RangePair<TValue>[] left = ranges[0..(midIndex - 1)];
			RangePair<TValue>[] right = ranges[(midIndex + 1)..];


			_range = mid;

			if (left.Length > 0) _left = new Node<TValue>(left);
			if (right.Length > 0) _right = new Node<TValue>(right);

			_max = ranges.Max(range => range.to);
		}

		public void Add(RangePair<TValue> range) {
			if (range.from < _range.from) {
				if (_left == null) {
					_left = new Node<TValue>(range);
				} else {
					_left.Add(range);
				}
			} else {
				if (_right == null) {
					_right = new Node<TValue>(range);
				} else {
					_right.Add(range);
				}
			}

			_max = Math.Max(_max, range.to);
		}



		public IntervalTree.Node<TValue>? Search(float key) {
			if (_range.Contains(key)) return this;

			if (_left != null && key < _left._max) {
				return _left.Search(key);
			}

			if (_right != null) {
				return _right.Search(key);
			}

			return null;
		}

		public IEnumerable<RangePair<TValue>> GetAll() {
			if (_left != null) {
				foreach (RangePair<TValue> range in _left.GetAll()) {
					yield return range;
				}
			}

			yield return _range;

			if (_right != null) {
				foreach (RangePair<TValue> range in _right.GetAll()) {
					yield return range;
				}
			}
		}

		public IEnumerable<RangePair<TValue>> GetAllThatContains(float key) {
			if (_range.Contains(key)) {
				yield return _range;
			}

			if (_left != null) {
				foreach (RangePair<TValue> range in _left.GetAllThatContains(key)) {
					yield return range;
				}
			}

			if (_right != null) {
				foreach (RangePair<TValue> range in _right.GetAllThatContains(key)) {
					yield return range;
				}
			}
		}
	}
}

