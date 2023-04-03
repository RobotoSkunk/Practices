// using System;
using System.Diagnostics;


public struct Vector2 {
	public float x;
	public float y;

	public Vector2(float x, float y) {
		this.x = x;
		this.y = y;
	}

	override public string ToString() {
		return "(" + x.ToString("0.00") + ", " + y.ToString("0.00") + ")";
	}
}

public class KDNode {
	public KDNode? left;
	public KDNode? right;
	public KDNode? parent;
	public Vector2 position;
	public bool depth;

	public KDNode(Vector2 position) {
		this.position = position;
		this.depth = true;
	}
	public KDNode(Vector2 position, bool depth) {
		this.position = position;
		this.depth = depth;
	}
	public KDNode(Vector2 position, bool depth, KDNode? parent) {
		this.position = position;
		this.depth = depth;
		this.parent = parent;
	}

	public void Insert(Vector2 position) {
		bool condition = depth ? position.x < this.position.x : position.y < this.position.y;

		if (condition) {
			if (left == null) {
				left = new KDNode(position, !depth, this);
			} else {
				left.Insert(position);
			}
		} else {
			if (right == null) {
				right = new KDNode(position, !depth, this);
			} else {
				right.Insert(position);
			}
		}
	}


	public KDNode? FindMinMaxScalar(float point) {
		if (point >= position.x && point <= position.y) {
			return this;
		}

		// if (left != null) {
		// 	if (point >= left.position.x && point <= left.position.y) {
		// 		return this;
		// 	}
		// }

		// if (right != null) {
		// 	if (point >= right.position.x && point <= right.position.y) {
		// 		return this;
		// 	}
		// }

		if (depth) {
			if (point < position.x && left != null) {
				return left.FindMinMaxScalar(point);
			} else if (right != null) {
				return right.FindMinMaxScalar(point);
			}
		} else {
			if (point < position.y && left != null) {
				return left.FindMinMaxScalar(point);
			} else if (right != null) {
				return right.FindMinMaxScalar(point);
			}
		}

		return null;
	}


	public IEnumerable<KDNode> GetNodes() {
		if (left != null) {
			foreach (var KDNode in left.GetNodes()) {
				yield return KDNode;
			}
		}

		yield return this;

		if (right != null) {
			foreach (var KDNode in right.GetNodes()) {
				yield return KDNode;
			}
		}
	}

	public IEnumerable<KDNode> GetTrueNodes(float point) {
		if (left != null) {
			foreach (var KDNode in left.GetTrueNodes(point)) {
				yield return KDNode;
			}
		}

		if (right != null) {
			foreach (var KDNode in right.GetTrueNodes(point)) {
				yield return KDNode;
			}
		}

		if (point >= position.x && point <= position.y) {
			yield return this;
		}
	}
}


namespace Testing {
	class Program {
		public static void Main(string[] args) {
			for (int i = 0; i < 10; i++) {
				Task.Run(() => {
					Console.WriteLine("Test " + (i) + " . . .");
					Test();
					Console.WriteLine("");
				}).Wait();
			}
		}

		private static void Test() {
			Random random = new Random();
			KDNode? tree = null;

			float _min = 0;
			float _max = 0;


			Stopwatch stopwatch1 = new Stopwatch();
			Stopwatch stopwatch2 = new Stopwatch();


			stopwatch1.Start();
			for (int i = 0; i < 1500; i++) {
				float min = random.NextSingle() * 1000f;
				float max = min + random.NextSingle() * 1000f;

				if (tree == null) {
					tree = new KDNode(new Vector2(min, max));
				} else {
					tree.Insert(new Vector2(min, max));
				}

				if (min < _min) _min = min;
				if (max > _max) _max = max;
			}
			stopwatch1.Stop();

			if (tree != null) {
				float found = 0;
				float trueValues = 0;
				float falseValues = 0;
				float missed = 0;
				float number = 0;


				float point = random.NextSingle() * (_max - _min) + _min;

				List<Vector2> list = new List<Vector2>();

				stopwatch2.Start();
				var minMax = tree.FindMinMaxScalar(point);


				if (minMax != null) {
					KDNode target = minMax;
					if (minMax.parent != null) {
						target = minMax.parent;
					}

					foreach (var KDNode in target.GetTrueNodes(point)) {
						if (KDNode.position.x <= point && KDNode.position.y >= point) {
							found++;
							list.Add(KDNode.position);
						} else {
							falseValues++;
						}

						number++;
					}

					stopwatch2.Stop();

					Console.WriteLine("Time Inserting: " + stopwatch1.ElapsedMilliseconds + "ms");
					Console.WriteLine("Time Searching: " + stopwatch2.ElapsedMilliseconds + "ms");
					Console.WriteLine("Calculating...");

					foreach (var KDNode in tree.GetNodes()) {
						if (KDNode.position.x <= point && KDNode.position.y >= point) {
							trueValues++;

							if (!list.Contains(KDNode.position)) {
								missed++;
							}
						}
					}

					Console.WriteLine($"Success: ({found} / {trueValues}) {(found / trueValues * 100f).ToString("0.00")}%");
					Console.WriteLine($"Failture: ({falseValues} / {number}) {(falseValues / number * 100f).ToString("0.00")}%");
					Console.WriteLine($"Lost: ({missed} / {trueValues}) {(missed / trueValues * 100f).ToString("0.00")}%");
				} else {
					Console.WriteLine("No values found!");
					stopwatch2.Stop();
					Console.WriteLine("Time Inserting: " + stopwatch1.ElapsedMilliseconds + "ms");
					Console.WriteLine("Time Searching: " + stopwatch2.ElapsedMilliseconds + "ms");
				}
			}
		}
	}
}

