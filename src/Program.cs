/*
	This file is just a public test file. It is not part of any project... yet.

	This interval tree is inspired on RangeTree by mbuchetics on GitHub (https://github.com/mbuchetics/RangeTree).
*/


using System.Diagnostics;




namespace Testing {
	static class Program {
		static readonly int tests = 100;
		static readonly int totalRanges = 15000;


		static void Main(string[] args) {
			float best = 0f, worst = 1f;
			int totalSuccess = 0;

			for (int i = 0; i < tests; i++) {
				Console.WriteLine($"Test {i + 1}");

				float result = Test();
				if (result > best) best = result;
				if (result < worst) worst = result;

				if (result == 1f) totalSuccess++;

				Console.WriteLine("");
			}

			Console.WriteLine($"Success rate: {((float)totalSuccess / (float)tests * 100f).ToString("0.00")}%");
			Console.WriteLine($"Best: {(best * 100f).ToString("0.00")}%");
			Console.WriteLine($"Worst: {(worst * 100f).ToString("0.00")}%");
		}


		static float Test() {
			// Start the stopwatches
			Stopwatch buildWatch = new Stopwatch();
			Stopwatch searchWatch = new Stopwatch();


			// Initialize the tree
			IntervalTree<int> tree = new IntervalTree<int>();
			Random random = new Random();
			float key = random.NextSingle() * 2000f;
			int found = 0, expected = 0;


			// Add some random ranges to the tree
			buildWatch.Start();
			for (int i = 0; i < totalRanges; i++) {
				float min = random.NextSingle() * 1000f;
				float max = min + random.NextSingle() * 1000f;

				RangePair<int> range = new RangePair<int>(min, max, i);

				tree.Add(range);
				if (range.Contains(key)) expected++;
			}
			buildWatch.Stop();


			// Search for all ranges that contain the key
			searchWatch.Start();
			IntervalTree.Node<int>? node = tree.Search(key);
			searchWatch.Stop();


			if (node != null) {
				foreach (RangePair<int> range in node.GetAllThatContains(key)) {
					found++;
				}
			}


			// Print the results
			Console.WriteLine($"Build time: {buildWatch.ElapsedMilliseconds}ms");
			Console.WriteLine($"Search time: {searchWatch.ElapsedMilliseconds}ms");
			Console.WriteLine($"Found {found} / {expected} ranges");

			if (expected == 0) return 1f;
			return (float)found / (float)expected;
		}
	}
}

