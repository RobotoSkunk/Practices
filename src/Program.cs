/*
	This file is just a public test file. It is not part of any project... yet.

	This interval tree is inspired on RangeTree by mbuchetics on GitHub (https://github.com/mbuchetics/RangeTree).
*/


using System.Diagnostics;




namespace Testing {
	static class Program {
		static readonly int tests = 100;
		static readonly int totalRanges = 10000;


		static void Main(string[] args) {
			float success = 1f, best = 0f, worst = 1f;

			for (int i = 0; i < tests; i++) {
				Console.WriteLine($"Test {i + 1}");

				float result = Test();
				success += result;
				if (result > best) best = result;
				if (result < worst) worst = result;

				Console.WriteLine("");
			}

			Console.WriteLine($"Success rate: {(success / (float)tests * 100f).ToString("0.00")}%");
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


			// Add 1000 random ranges to the tree
			for (int i = 0; i < totalRanges; i++) {
				float min = random.NextSingle() * 1000f;
				float max = min + random.NextSingle() * 1000f;

				RangePair<int> range = new RangePair<int>(min, max, i);

				tree.Add(range);
				if (range.Contains(key)) expected++;
			}


			// Build the tree and search for a random key
			buildWatch.Start();
			tree.Build();
			buildWatch.Stop();


			// Search for all ranges that contain the key
			searchWatch.Start();
			foreach (RangePair<int> range in tree.Search(key)) {
				// Console.WriteLine(range);
				// Do nothing...
				found++;
			}
			searchWatch.Stop();


			// Print the results
			Console.WriteLine($"Build time: {buildWatch.ElapsedMilliseconds}ms ({buildWatch.Elapsed.TotalNanoseconds} ns)");
			Console.WriteLine($"Search time: {searchWatch.ElapsedMilliseconds}ms ({searchWatch.Elapsed.TotalNanoseconds} ns)");
			Console.WriteLine($"Found {found} / {expected} ranges");

			if (expected == 0) return 1f;
			return (float)found / (float)expected;
		}
	}
}

