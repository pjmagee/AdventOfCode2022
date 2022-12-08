<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

public static string PuzzleInput =
"""
30373
25512
65332
33549
35390
""";

async Task Main()
{
	var baseAddress = new Uri("https://adventofcode.com");
	var COOKIE = Util.GetPassword("adventofcode.cookie");

	using (var handler = new HttpClientHandler())
	using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
	{
		handler.CookieContainer.Add(baseAddress, new Cookie("session", COOKIE));

		var puzzleInput = await client.GetStringAsync("/2022/day/8/input");

		var lines = puzzleInput.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);				
		var trees = new Tree[lines.Length, lines[0].Length];
		
		for(int y = 0; y < lines.Length; y++)
		{	
			var line = lines[y];
			
			for(int x = 0; x < line.Length; x++)
			{
				trees[y, x] = new Tree() { Point = (x, y), Height = int.Parse(line[x].ToString()), Trees = trees };	
			}
		}
			
		trees
			.Cast<Tree>()
			.Count(t => t.IsVisible).Dump("Part 1");
			
		trees
			.Cast<Tree>()
			.Max(x => x.ScenicScore).Dump("Part 2");
	}
}

public class Tree : IEquatable<Tree>
{
	public (int X, int Y) Point { get; set; }
	public int Height { get; set; }
	public Tree[,] Trees { get; set; }

	public bool IsVisible => new[] { IsEdgeTree, IsVisibleLeft, IsVisibleRight, IsVisibleTop, IsVisibleBottom }.Any(x => x == true);

	public bool IsEdgeTree => Point.X == 0 || Point.Y == 0 || Point.X == Trees.GetLength(0) - 1 || Point.Y == Trees.GetLength(1) - 1;
	private bool IsVisibleLeft => Enumerable.Range(0, Point.X).Select(x => Trees[Point.Y, x]).Where(tree => tree != this).All(tree => tree.Height < Height);
	private bool IsVisibleRight => Enumerable.Range(Point.X, Trees.GetLength(1) - Point.X).Select(x => Trees[Point.Y, x]).Where(tree => tree != this).All(tree => tree.Height < Height);
	private bool IsVisibleTop => Enumerable.Range(0, Point.Y).Select(y => Trees[y, Point.X]).Where(tree => tree != this).All(tree => tree.Height < Height);
	private bool IsVisibleBottom => Enumerable.Range(Point.Y, Trees.GetLength(0) - Point.Y).Select(y => Trees[y, Point.X]).Where(tree => tree != this).All(tree => tree.Height < Height);

	public int ScenicScore => GetUp() * GetLeft() * GetRight() * GetDown();
	public string ScenicScoreCalculation => $"{GetUp()} * {GetLeft()} * {GetRight()} * {GetDown()}";

	private int GetUp()
	{
		if(IsEdgeTree) return 0;
		
		int distance = 0;		
		Tree current = this;

		do
		{
			current = Trees[current.Point.Y - 1, current.Point.X];		
			distance++;
			if (current.IsEdgeTree || current.Height >= Height) break;
		}
		while(true);

		return distance;
	}

	private int GetLeft()
	{
		if(IsEdgeTree) return 0;
		
		int distance = 0;
		Tree current = this;

		do
		{
			current = Trees[current.Point.Y, current.Point.X - 1];
			distance++;
			if (current.IsEdgeTree || current.Height >= Height) break;
		}
		while (true);

		return distance;
	}

	private int GetDown()
	{
		if(IsEdgeTree) return 0;
		
		int distance = 0;
		
		Tree current = this;

		do
		{
			current = Trees[current.Point.Y + 1, current.Point.X];
			distance++;
			if (current.IsEdgeTree || current.Height >= Height) break;
		}
		while (true);

		return distance;
	}

	private int GetRight()
	{
		if(IsEdgeTree) return 0;
		
		int distance = 0;
		Tree current = this;

		do
		{
			current = Trees[current.Point.Y, current.Point.X + 1];
			distance++;
			if (current.IsEdgeTree || current.Height >= Height) break;
		}
		while (true);

		return distance;
	}

	public bool Equals(Tree other) => this.Point.X.Equals(other.Point.X) && this.Point.Y.Equals(other.Point.Y);

	private object ToDump() => new { X = Point.X, Y = Point.Y, Height, IsVisible, ScenicScore, ScenicScoreCalculation };
}