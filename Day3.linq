<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

async Task Main()
{
	var baseAddress = new Uri("https://adventofcode.com");
	var COOKIE = Util.GetPassword("adventofcode.cookie");
	
	using (var handler = new HttpClientHandler())
	using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
	{
		handler.CookieContainer.Add(baseAddress, new Cookie("session", COOKIE));
		var puzzleInput = await client.GetStringAsync("/2022/day/3/input");
				
		var rucksacks = puzzleInput
							.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
							.Select(rucksack => new Rucksack(rucksack));
		
		// Part 1
		rucksacks
			 .Select(x => x.Priorities.Sum(p => p.Priority))
			 .Sum()
			 .Dump("Part 1 Priority Sum");
			 
		// Part 2
		// every set of three lines in your list corresponds to a single group		
		// each group can have a different badge item type
		rucksacks
			.Chunk(3)
			.Sum(elves => Rucksack.GetPriority(elves[0].ItemTypes.Intersect(elves[1].ItemTypes).Intersect(elves[2].ItemTypes).Single()))
			.Dump("Sum of the priorities of item types");
		
	}
}

public class Rucksack
{
	private char[] rucksack;
	
	private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

	private static string Letters => $"{Alphabet}{Alphabet.ToUpper()}";
	
	
	private string ContainerA => string.Join(string.Empty, rucksack.Take(rucksack.Length / 2));
	private string ContainerB => string.Join(string.Empty, rucksack.Skip(rucksack.Length / 2));

	public List<(char ItemType, int Priority)> Priorities => CalculatePriorities();
	
	public static int GetPriority(char letter) => Letters.IndexOf(letter) + 1;
	
	public HashSet<char> ItemTypes => rucksack.ToHashSet();
		
	public Rucksack(string contents)
	{
		rucksack = contents.ToCharArray();
	}
		
	private List<(char ItemType, int Priority)> CalculatePriorities()
	{
		List<(char ItemType, int Priority)> priorities = new();
		
		foreach(var letter in ContainerA.Distinct())
		{
			if (ContainerB.Contains(letter, StringComparison.CurrentCulture))
			{
				priorities.Add((letter, GetPriority(letter)));	
			}
		}
		
		priorities.Sort(new PriorityComparer());
		
		return priorities;
	}

	public class PriorityComparer : IComparer<(char ItemType, int Priority)>
	{
		public int Compare((char ItemType, int Priority) x, (char ItemType, int Priority) y) => x.Priority.CompareTo(y.Priority);
	}
}

