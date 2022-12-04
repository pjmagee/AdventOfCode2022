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
		
		var puzzleInput = await client.GetStringAsync("/2022/day/4/input");
		 
		//var puzzleInput = 
		//
		//"""
		//5-7,7-9
		//2-8,3-7
		//6-6,4-6
		//2-6,4-8
		//""";
				
		/* In how many assignment pairs does one range fully contain the other? */
		puzzleInput
			.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.Select(pair => PairParser.GetAssignmentPairs(pair))
			.Count(pair => pair.CalculateOneAssignmentFullyContainsTheOther())
			.Dump("Part 1");

		puzzleInput
			.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.Select(pair => PairParser.GetAssignmentPairs(pair))
			.Count(pair => pair.CalculateOverlaps() > 0)
			.Dump("Part 2");
	}
}

/*
	Every section has a unique ID number, and each Elf is assigned a range of section IDs.
*/
public readonly struct Section : IEquatable<Section>
{
	public int ID { get; }

	public static bool operator ==(Section x, Section y) => x.ID == y.ID;
	public static bool operator !=(Section x, Section y) => x.ID != y.ID;

	public Section(int id)
	{
		ID = id;
	}

	public bool Equals(Section other) => this == other;
}

public class AssignmentPair
{
	/*
		However, as some of the Elves compare their section assignments with each other, they've noticed that many of the assignments overlap
	*/
	public IEnumerable<Section> Elf1 { get; set; }
	public IEnumerable<Section> Elf2 { get; set; }
	
	/*
		To try to quickly find overlaps and reduce duplicated effort
	*/
	public bool CalculateOneAssignmentFullyContainsTheOther()
	{
		return Elf1.All(n => Elf2.Contains(n)) || Elf2.All(n => Elf1.Contains(n));
	}

	public int CalculateOverlaps()
	{
		if (Elf1.Any(n => Elf2.Contains(n)))
		{
			return Elf1.Intersect(Elf2).Count();
		}
		else if(Elf2.Any(n => Elf1.Contains(n)))
		{
			return Elf2.Intersect(Elf1).Count();
		}
		
		return 0;
	}
}


public class PairParser
{
	public static AssignmentPair GetAssignmentPairs(string pair)
	{
		var split = pair.Split(',');		
		
		return new AssignmentPair 
		{
			Elf1 = GetSections(split[0]),
			Elf2 = GetSections(split[1]) 
		};
	}
	
	private static IEnumerable<Section> GetSections(string assignedSections)
	{
		var split = assignedSections.Split('-');
		var start = int.Parse(split[0]);
		var end = int.Parse(split[1]);		
		var count = end - start + 1;	
		return Enumerable.Range(start, count).Select(id => new Section(id));
	}
}
