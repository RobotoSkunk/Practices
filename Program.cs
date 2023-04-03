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

	public static float Distance(Vector2 p1, Vector2 p2) {
		return (float)System.Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
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

		// if (left != null && right != null) {
		// 	if (point >= left.position.x && point <= left.position.y ||
		// 		point >= right.position.x && point <= right.position.y) {
		// 		return this;
		// 	}
		// }

		bool condition = depth ? point < position.x : point < position.y;


		if (condition && left != null) {
			return left.FindMinMaxScalar(point);
		} else if (right != null) {
			return right.FindMinMaxScalar(point);
		}

		return null;
	}
	public KDNode? FindNode(Vector2 position) {
		if (this.position.x == position.x && this.position.y == position.y) {
			return this;
		}
		bool condition = depth ? position.x < this.position.x : position.y < this.position.y;


		if (condition && left != null) {
			return left.FindNode(position);
		} else if (right != null) {
			return right.FindNode(position);
		}

		return null;
	}


	public IEnumerable<KDNode> GetNodes() {
		if (left != null) {
			foreach (var node in left.GetNodes()) {
				yield return node;
			}
		}

		yield return this;

		if (right != null) {
			foreach (var node in right.GetNodes()) {
				yield return node;
			}
		}
	}
	public IEnumerable<KDNode> GetTrueNodes(float point) {
		if (left != null) {
			foreach (var node in left.GetTrueNodes(point)) {
				yield return node;
			}
		}

		if (right != null) {
			foreach (var node in right.GetTrueNodes(point)) {
				yield return node;
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
				Console.WriteLine("Test " + (i + 1) + " . . .");
				Test();
				Console.WriteLine("");
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
			for (int i = 0; i < 15000; i++) {
				float min = random.NextSingle() * 10f;
				float max = min + random.NextSingle() * 10f;

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


				stopwatch2.Start();
				var minMax = tree.FindMinMaxScalar(point);


				if (minMax != null) {
					KDNode target = minMax;
					if (minMax.parent != null) {
						target = minMax.parent;
					}

					KDNode list = new KDNode(minMax.position);


					foreach (var node in target.GetTrueNodes(point)) {
						if (node.position.x <= point && node.position.y >= point) {
							found++;
							list.Insert(node.position);
						} else {
							falseValues++;
						}

						number++;
					}

					stopwatch2.Stop();

					Console.WriteLine("Time Inserting: " + stopwatch1.ElapsedMilliseconds + "ms");
					Console.WriteLine("Time Searching: " + stopwatch2.ElapsedMilliseconds + "ms");
					Console.WriteLine("Calculating...");

					foreach (var node in tree.GetNodes()) {
						if (node.position.x <= point && node.position.y >= point) {
							trueValues++;

							if (list.FindNode(node.position) == null) {
								missed++;
							}
						}
					}

					Console.WriteLine($"Success: ({found} / {trueValues}) {(found / trueValues * 100f).ToString("0.00")}%");
					Console.WriteLine($"Failture: ({falseValues} / {number}) {(falseValues / number * 100f).ToString("0.00")}%");
					Console.WriteLine($"Lost: ({missed} / {trueValues}) {(missed / trueValues * 100f).ToString("0.00")}%");
				} else {
					stopwatch2.Stop();
					Console.WriteLine("Time Inserting: " + stopwatch1.ElapsedMilliseconds + "ms");
					Console.WriteLine("Time Searching: " + stopwatch2.ElapsedMilliseconds + "ms");
					Console.WriteLine("No values found!");
				}
			}
		}
	}
}

