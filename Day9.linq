<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
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
		
		var puzzleInput = await client.GetStringAsync("/2022/day/9/input");

		Rope rope = new Rope();
		rope.MoveHead(Move.Enumerate(puzzleInput));		
		rope.Tail.Visited.Count.Dump("Part 1");		
		
		rope = new Rope(length: 10);
		rope.MoveHead(Move.Enumerate(puzzleInput));
		rope.Tail.Visited.Count.Dump("Part 2");
	}
}

public class Point
{
	public int X { get; private set; }
	public int Y { get; private set; }
	
	public HashSet<(int, int)> Visited = new();

	public Point() : this(0, 0)
	{

	}

	public Point(int y, int x)
	{
		SetPosition(y, x);
	}

	public void SetPosition(int y, int x)
	{
		this.Y = y;
		this.X = x;
		this.Visited.Add((y, x));
	}
}

public class Head : Point
{
	public Tail Child { get; }
	
	public Tail Tail
	{
		get
		{
			var tail = Child;
			
			while (tail.Child != null)
			{
				tail = tail.Child;
			}
			
			return tail;
		}
	}

	public Head() : base()
	{
		this.Child = new Tail(this);
	}

	public Head(int length) : this()
	{
		this.Child = new Tail(this, length - 1);
	}

	public void Move(Move move)
	{
		this.SetPosition(this.Y + move.Y, this.X + move.X);
		this.Child.Follow();
	}
}

public class Tail : Point
{
	public Point Head { get; }
	public Tail? Child { get; }

	public Tail(Point parent) : base()
	{
		this.Head = parent;
	}

	public Tail(Point parent, int length) : this(parent)
	{
		if (length > 1)
		{
			this.Child = new Tail(this, length - 1);
		}
	}

	public void Follow()
	{
		if (IsAdjacent()) return;
		int yDist = Math.Sign(Head.Y - this.Y);
		int xDist = Math.Sign(Head.X - this.X);
		this.SetPosition(this.Y + yDist, this.X + xDist);
		this.Child?.Follow();
	}

	private bool IsAdjacent()
	{
		int yDist = Math.Abs(Head.Y - this.Y);
		int xDist = Math.Abs(Head.X - this.X);
		int dist = yDist + xDist;
		return dist < 2 || (yDist == 1 && xDist == 1) || (yDist == 1 && xDist == 1);
	}
}

public class Rope
{
	public Head Head { get; set; }
	public Tail Tail { get; set; }
	
	public void MoveHead(IEnumerable<Move> move)
	{
		foreach(var m in move)
		{
			Head.Move(m);
		}
	}

	public Rope(int length = 1)
	{
		Head = new(length);
		Tail = Head.Tail;
	}
}

public record Move(int Y, int X)
{
	public static IEnumerable<Move> Enumerate(string input)
	{
		foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
		{
			var tokens = line.Split(" ");
			var direction = tokens[0] switch { "U" => Direction.Up, "D" => Direction.Down, "L" => Direction.Left, "R" => Direction.Right };
			var distance = int.Parse(tokens[1]);

			var move = direction switch
			{
				Direction.Right => new Move(0, 1),
				Direction.Left => new Move(0, -1),
				Direction.Up => new Move(-1, 0),
				Direction.Down => new Move(1, 0)
			};

			for (var i = 0; i < distance; i++)
				yield return move;
		}
	}
}

public enum Direction { Up, Left, Down, Right }
