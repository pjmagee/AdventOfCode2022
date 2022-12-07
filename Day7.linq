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

		var puzzleInput = await client.GetStringAsync("/2022/day/7/input");

		Node root = new Node() { Name = "/" };
		Node current = null;
				
		foreach(var line in puzzleInput.Split('\n'))
		{
			var segments = line.Split(' ', StringSplitOptions.TrimEntries);
			
			if (segments[0] == "$")
			{
				if (segments[1] == "cd")
				{
					if (segments[2] == "/")
					{
						current = root;
					}
					else if (segments[2] == "..")
					{
						current = current.Parent;	
					}
					else
					{
						current = current.Children.Find(n => n.Name == segments[2]);
					}
				}
				else if (segments[1] == "ls")
				{
					
				}
			}
			else if (segments[0] == "dir")
			{
				current.AddDir(new Node(){ Name = segments[1] });
			}			
			else if (int.TryParse(segments[0], out var size))
			{
				current.AddFile(new Node() { Name = segments[1], Size = size });
			}
		}

		FileSystemInfo fsi = new FileSystemInfo(totalDiskSpace: 70_000_000);		
		fsi.Scan(root);		
		fsi.GetTotalOfDirectoriesWithMaxSize(100_000).Dump("Part 1");		
		fsi.GetDirectoryToDeleteForTotalUnused(30_000_000).Dump("Part 2");
		
	}
}

public class FileSystemInfo
{
	public int TotalDiskSpace { get; }
	
	public Dictionary<Node, int> Visited = new Dictionary<Node, int>();
	
	public FileSystemInfo(int totalDiskSpace)
	{
		TotalDiskSpace = totalDiskSpace;
	}

	public int Depth(Node node)
	{
		int depth = 0;

		while (node.Parent != null)
		{
			node = node.Parent;
			depth++;
		}

		return depth;
	}
	
	public (string Name, int Size) GetDirectoryToDeleteForTotalUnused(int size)
	{
		var unused = GetUnused();
		
		var smallestToDelete = Visited.Keys
									  .Where(node => node.IsDirectory && (unused + node.Size) >= size)
									  .OrderByDescending(x => x.Size)
									  .Last(); // Pick the smallest directory that would meet the criteria
			
		return (Name: smallestToDelete.Name, Size: smallestToDelete.Size);
	}
	
	public int GetUnused() => TotalDiskSpace - Visited.Keys.Single(n => n.Parent is null).Size;
	
	public int GetTotalOfDirectoriesWithMaxSize(int size) => Visited.Keys.Where(n => n.Parent is not null && n.Size <= size && n.Children.Any()).Sum(x => x.Size);

	public void Visit(Node node) => Visited.Add(node, Depth(node));

	public void Scan(Node root)
	{
		this.Calculate(root, -1);
	}

	private void Calculate(Node node, int depth = -1)
	{
		depth = depth == -1 ? Depth(node) : depth;

		Visited.Add(node, depth);

		foreach (var n in node.Children)
		{
			Calculate(n, depth + 1);
		}
	}
}

public class Node
{

	public Node? Parent { get; set; }
	
	public List<Node> Children { get; set; } = new();
	
	public string Name { get; set; }	
	
	public bool IsDirectory => size is null;
	
	private int? size;
	
	public int Size 
	{ 
		get => size ?? Children.Sum(c => c.Size); 
		set => size = value;
	}
	
	public void AddDir(Node node)
	{
		node.Parent = this;
		Children.Add(node);		
	}
	
	public void AddFile(Node node)
	{
		node.Parent = this;
		Children.Add(node);		
	}
}