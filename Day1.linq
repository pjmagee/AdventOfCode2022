<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var baseAddress = new Uri("https://adventofcode.com");
	var COOKIE = Util.GetPassword("adventofcode.cookie");

	using (var handler = new HttpClientHandler())
	using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
	{
		handler.CookieContainer.Add(baseAddress, new Cookie("session", COOKIE));
		var puzzleInput = await client.GetStringAsync("/2022/day/1/input");

		Dictionary<int, List<int>> elves = new();
		int current = 0;

		foreach (var line in puzzleInput.Split('\n', StringSplitOptions.TrimEntries))
		{
			if (!elves.TryGetValue(current, out var items))
			{
				items = new List<int>();
				elves.Add(current, items);
			}

			if (string.IsNullOrWhiteSpace(line))
			{
				current++;
			}
			else
			{
				items.Add(int.Parse(line));
			}
		}

		// Calculation for Answer 
		elves.Select(x => new
		{
			Elf = x.Key,
			Total = x.Value.Sum()
		})
		.OrderByDescending(x => x.Total)
		.Take(3)
		.Sum(x => x.Total)
		.Dump();
	}


	// Load data properly 
	
	
}

// You can define other methods, fields, classes and namespaces here