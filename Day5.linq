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

		var puzzleInput = await client.GetStringAsync("/2022/day/5/input");
		
		// Transpose?		
		List<CrateStack> CreateStacks()
		{
			List<CrateStack> stacks = new List<CrateStack>()
			{
				new CrateStack(new[] { 'D', 'T', 'W', 'F', 'J', 'S', 'H', 'N' }),
				new CrateStack(new[] { 'H', 'R', 'P', 'Q', 'T', 'N', 'B', 'G' }),
				new CrateStack(new[] { 'L', 'Q', 'V' }),
				new CrateStack(new[] { 'N', 'B', 'S', 'W', 'R', 'Q' }),
				new CrateStack(new[] { 'N', 'D', 'F', 'T', 'V', 'M', 'B' }),
				new CrateStack(new[] { 'M', 'D', 'B', 'V', 'H', 'T', 'R' }),
				new CrateStack(new[] { 'D', 'B', 'Q', 'J' }),
				new CrateStack(new[] { 'D', 'N', 'J', 'V', 'R', 'Z', 'H', 'Q' }),
				new CrateStack(new[] { 'B', 'N', 'H', 'M', 'S' })
			};
			
			return stacks;
		}


		var instructions = puzzleInput.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).SkipWhile(line => !line.StartsWith("move")).Select(line => new Instruction(line));

		CrateMover part1 = new CrateMover(CreateStacks(), instructions);				
		part1.Operate();		
		part1.TopCrates.Dump("Part 1");
		
		CrateMover9001 part2 = new CrateMover9001(CreateStacks(), instructions);
		part2.Operate();
		part2.TopCrates.Dump("Part 2");
	}
}

#region Input 


static string PuzzleInput =

"""
[N] [G]                     [Q]    
[H] [B]         [B] [R]     [H]    
[S] [N]     [Q] [M] [T]     [Z]    
[J] [T]     [R] [V] [H]     [R] [S]
[F] [Q]     [W] [T] [V] [J] [V] [M]
[W] [P] [V] [S] [F] [B] [Q] [J] [H]
[T] [R] [Q] [B] [D] [D] [B] [N] [N]
[D] [H] [L] [N] [N] [M] [D] [D] [B]
 1   2   3   4   5   6   7   8   9 

move 3 from 1 to 2
move 1 from 7 to 1
move 1 from 6 to 5
move 5 from 5 to 9
move 2 from 5 to 2
move 1 from 6 to 8
move 1 from 5 to 7
move 5 from 4 to 6
move 1 from 7 to 6
move 1 from 2 to 4
move 5 from 2 to 6
move 2 from 1 to 5
move 2 from 1 to 9
move 16 from 6 to 4
move 6 from 8 to 3
move 7 from 2 to 4
move 5 from 9 to 3
move 1 from 1 to 4
move 1 from 1 to 3
move 3 from 7 to 4
move 2 from 5 to 4
move 31 from 4 to 8
move 22 from 8 to 4
move 9 from 3 to 6
move 7 from 9 to 5
move 4 from 5 to 6
move 6 from 3 to 2
move 2 from 6 to 7
move 5 from 2 to 7
move 1 from 2 to 4
move 1 from 7 to 5
move 4 from 5 to 4
move 2 from 6 to 9
move 2 from 4 to 6
move 7 from 6 to 4
move 2 from 6 to 1
move 1 from 6 to 8
move 8 from 8 to 1
move 1 from 7 to 6
move 4 from 1 to 5
move 9 from 4 to 8
move 4 from 1 to 7
move 3 from 5 to 3
move 2 from 1 to 9
move 1 from 3 to 2
move 1 from 9 to 8
move 1 from 2 to 1
move 1 from 1 to 8
move 1 from 5 to 1
move 2 from 3 to 1
move 2 from 6 to 9
move 19 from 4 to 1
move 4 from 4 to 2
move 6 from 1 to 4
move 1 from 2 to 4
move 4 from 4 to 3
move 7 from 7 to 3
move 7 from 8 to 2
move 2 from 7 to 4
move 3 from 2 to 1
move 8 from 8 to 2
move 3 from 9 to 1
move 2 from 9 to 1
move 10 from 2 to 7
move 4 from 3 to 1
move 1 from 8 to 3
move 1 from 4 to 5
move 1 from 3 to 6
move 1 from 2 to 1
move 10 from 1 to 3
move 1 from 4 to 7
move 1 from 6 to 4
move 7 from 3 to 2
move 5 from 2 to 8
move 11 from 7 to 2
move 3 from 4 to 3
move 1 from 4 to 3
move 5 from 8 to 9
move 17 from 2 to 4
move 11 from 1 to 5
move 4 from 1 to 3
move 5 from 9 to 2
move 4 from 2 to 1
move 3 from 5 to 7
move 6 from 5 to 3
move 1 from 5 to 8
move 6 from 1 to 8
move 3 from 8 to 5
move 1 from 1 to 4
move 1 from 7 to 2
move 15 from 3 to 4
move 1 from 1 to 3
move 10 from 3 to 9
move 2 from 7 to 4
move 1 from 2 to 8
move 21 from 4 to 9
move 1 from 2 to 3
move 1 from 8 to 1
move 9 from 4 to 2
move 1 from 1 to 5
move 5 from 2 to 7
move 2 from 8 to 5
move 1 from 8 to 1
move 2 from 2 to 8
move 2 from 4 to 9
move 24 from 9 to 5
move 3 from 4 to 1
move 2 from 2 to 5
move 12 from 5 to 1
move 10 from 1 to 5
move 23 from 5 to 6
move 8 from 9 to 1
move 3 from 8 to 1
move 1 from 1 to 2
move 1 from 3 to 7
move 11 from 6 to 1
move 1 from 2 to 4
move 6 from 6 to 8
move 4 from 6 to 7
move 1 from 7 to 3
move 1 from 3 to 4
move 23 from 1 to 8
move 1 from 4 to 2
move 1 from 2 to 1
move 1 from 6 to 7
move 6 from 5 to 3
move 1 from 7 to 8
move 1 from 1 to 8
move 1 from 9 to 3
move 6 from 7 to 2
move 3 from 5 to 9
move 5 from 2 to 3
move 28 from 8 to 3
move 4 from 1 to 9
move 5 from 9 to 5
move 2 from 8 to 5
move 1 from 9 to 4
move 2 from 7 to 5
move 1 from 4 to 2
move 1 from 4 to 8
move 2 from 8 to 3
move 6 from 5 to 2
move 1 from 7 to 2
move 39 from 3 to 2
move 2 from 3 to 8
move 1 from 9 to 6
move 2 from 2 to 9
move 2 from 9 to 6
move 1 from 8 to 1
move 1 from 1 to 6
move 5 from 6 to 9
move 2 from 5 to 8
move 20 from 2 to 4
move 2 from 4 to 8
move 2 from 8 to 3
move 3 from 3 to 1
move 22 from 2 to 5
move 2 from 9 to 1
move 3 from 1 to 7
move 1 from 2 to 6
move 1 from 2 to 9
move 1 from 1 to 8
move 2 from 7 to 9
move 1 from 6 to 8
move 1 from 2 to 7
move 1 from 1 to 3
move 1 from 9 to 8
move 1 from 8 to 5
move 3 from 8 to 7
move 3 from 7 to 8
move 15 from 4 to 1
move 1 from 4 to 3
move 10 from 1 to 6
move 3 from 8 to 1
move 5 from 9 to 4
move 7 from 5 to 1
move 4 from 6 to 3
move 15 from 5 to 2
move 4 from 6 to 4
move 7 from 2 to 1
move 6 from 4 to 6
move 1 from 5 to 9
move 1 from 5 to 7
move 1 from 3 to 5
move 11 from 1 to 8
move 3 from 4 to 6
move 4 from 1 to 5
move 1 from 2 to 5
move 2 from 8 to 3
move 11 from 6 to 1
move 1 from 3 to 7
move 1 from 9 to 8
move 6 from 5 to 8
move 3 from 8 to 4
move 1 from 4 to 5
move 3 from 3 to 1
move 9 from 8 to 2
move 2 from 1 to 5
move 11 from 2 to 5
move 1 from 3 to 6
move 2 from 8 to 5
move 3 from 4 to 6
move 1 from 8 to 3
move 2 from 1 to 9
move 1 from 3 to 8
move 16 from 5 to 7
move 3 from 1 to 6
move 1 from 3 to 5
move 1 from 6 to 7
move 1 from 9 to 4
move 1 from 5 to 4
move 1 from 3 to 2
move 1 from 1 to 2
move 3 from 4 to 9
move 1 from 2 to 7
move 2 from 8 to 3
move 6 from 2 to 8
move 11 from 1 to 3
move 6 from 3 to 1
move 4 from 3 to 2
move 2 from 3 to 1
move 1 from 1 to 3
move 4 from 8 to 4
move 4 from 8 to 2
move 11 from 7 to 2
move 9 from 7 to 5
move 1 from 7 to 3
move 4 from 5 to 7
move 14 from 2 to 3
move 17 from 3 to 7
move 2 from 5 to 2
move 1 from 5 to 7
move 1 from 5 to 6
move 4 from 6 to 7
move 8 from 1 to 2
move 2 from 6 to 4
move 1 from 6 to 8
move 6 from 4 to 1
move 1 from 8 to 5
move 6 from 7 to 8
move 5 from 8 to 3
move 12 from 2 to 1
move 1 from 8 to 4
move 4 from 3 to 1
move 4 from 2 to 4
move 3 from 9 to 3
move 3 from 3 to 2
move 1 from 3 to 2
move 3 from 4 to 1
move 2 from 5 to 7
move 22 from 1 to 8
move 17 from 8 to 6
move 21 from 7 to 6
move 3 from 2 to 8
move 3 from 1 to 5
move 3 from 5 to 2
move 2 from 4 to 6
move 7 from 6 to 5
move 1 from 9 to 4
move 14 from 6 to 4
move 5 from 8 to 3
move 1 from 6 to 3
move 3 from 3 to 9
move 2 from 9 to 1
move 2 from 7 to 1
move 16 from 6 to 8
move 2 from 6 to 7
move 1 from 2 to 7
move 1 from 3 to 8
move 7 from 4 to 1
move 2 from 7 to 2
move 4 from 4 to 7
move 5 from 2 to 4
move 1 from 7 to 3
move 3 from 5 to 8
move 1 from 7 to 5
move 12 from 1 to 6
move 3 from 7 to 2
move 7 from 4 to 2
move 3 from 3 to 2
move 1 from 4 to 2
move 1 from 9 to 8
move 8 from 6 to 8
move 12 from 2 to 4
move 5 from 5 to 2
move 11 from 4 to 9
move 3 from 6 to 3
move 2 from 4 to 2
move 4 from 2 to 6
move 5 from 2 to 8
move 12 from 8 to 4
move 20 from 8 to 5
move 13 from 5 to 3
move 1 from 8 to 5
move 5 from 5 to 9
move 16 from 9 to 1
move 9 from 4 to 5
move 12 from 3 to 9
move 5 from 6 to 5
move 9 from 9 to 7
move 14 from 1 to 4
move 14 from 4 to 1
move 15 from 5 to 7
move 4 from 8 to 2
move 3 from 4 to 3
move 3 from 1 to 8
move 1 from 5 to 9
move 1 from 5 to 3
move 3 from 9 to 8
move 4 from 3 to 4
move 1 from 4 to 6
move 20 from 7 to 2
move 2 from 3 to 8
move 3 from 7 to 2
move 4 from 2 to 1
move 1 from 6 to 7
move 3 from 4 to 2
move 2 from 2 to 3
move 4 from 3 to 4
move 1 from 8 to 1
move 3 from 8 to 1
move 2 from 7 to 8
move 1 from 4 to 5
move 14 from 2 to 5
move 6 from 1 to 5
move 1 from 4 to 3
move 15 from 1 to 4
move 1 from 8 to 2
move 1 from 9 to 5
move 4 from 8 to 7
move 13 from 5 to 6
move 1 from 8 to 1
move 2 from 7 to 9
move 12 from 6 to 4
move 1 from 3 to 6
move 1 from 1 to 6
move 4 from 5 to 2
move 5 from 5 to 6
move 2 from 6 to 2
move 1 from 7 to 5
move 2 from 6 to 9
move 1 from 5 to 9
move 16 from 2 to 5
move 17 from 4 to 1
move 3 from 1 to 3
move 1 from 2 to 6
move 2 from 6 to 1
move 3 from 3 to 1
move 14 from 1 to 8
move 3 from 5 to 2
move 4 from 8 to 2
move 3 from 4 to 5
move 15 from 5 to 3
move 1 from 7 to 6
move 3 from 1 to 8
move 2 from 3 to 7
move 1 from 1 to 2
move 1 from 7 to 6
move 4 from 2 to 8
move 2 from 6 to 2
move 1 from 7 to 6
move 3 from 8 to 2
move 12 from 8 to 6
move 1 from 5 to 6
move 3 from 2 to 5
move 2 from 2 to 5
move 4 from 6 to 5
move 4 from 3 to 5
move 1 from 8 to 4
move 11 from 6 to 4
move 6 from 3 to 1
move 2 from 9 to 8
move 20 from 4 to 5
move 1 from 4 to 9
move 2 from 3 to 8
move 1 from 3 to 8
move 17 from 5 to 8
move 5 from 5 to 9
move 9 from 5 to 1
move 2 from 6 to 7
move 23 from 8 to 2
move 2 from 7 to 5
move 3 from 9 to 4
move 16 from 2 to 4
move 11 from 1 to 8
move 4 from 5 to 8
move 11 from 2 to 6
move 2 from 6 to 1
move 5 from 9 to 5
move 5 from 5 to 6
move 5 from 8 to 6
move 1 from 6 to 7
move 7 from 8 to 1
move 12 from 1 to 2
move 1 from 9 to 5
move 1 from 1 to 3
move 1 from 1 to 4
move 1 from 5 to 3
move 1 from 3 to 6
move 1 from 8 to 2
move 18 from 6 to 2
move 1 from 6 to 2
move 2 from 8 to 3
move 3 from 3 to 8
move 18 from 4 to 9
move 11 from 9 to 2
move 2 from 9 to 6
move 2 from 4 to 1
move 1 from 1 to 5
move 1 from 5 to 4
move 1 from 4 to 8
move 42 from 2 to 1
move 3 from 9 to 3
move 1 from 8 to 1
move 1 from 3 to 4
move 3 from 8 to 7
move 1 from 4 to 1
move 2 from 3 to 2
move 17 from 1 to 6
move 15 from 6 to 3
move 2 from 9 to 7
move 1 from 3 to 6
move 2 from 7 to 6
move 2 from 2 to 4
move 1 from 2 to 3
move 1 from 4 to 9
move 1 from 4 to 1
move 1 from 6 to 3
move 20 from 1 to 9
move 6 from 1 to 9
move 7 from 9 to 3
move 20 from 9 to 1
move 1 from 6 to 7
move 2 from 6 to 7
move 1 from 6 to 5
move 1 from 6 to 8
move 4 from 7 to 3
move 3 from 7 to 2
move 1 from 6 to 4
move 1 from 2 to 1
move 1 from 4 to 9
move 21 from 3 to 2
move 5 from 3 to 8
move 1 from 5 to 1
move 2 from 8 to 7
move 4 from 8 to 3
move 4 from 2 to 5
move 19 from 2 to 3
move 1 from 9 to 2
move 23 from 3 to 2
move 2 from 7 to 4
move 3 from 5 to 9
move 16 from 2 to 1
move 1 from 5 to 4
move 1 from 9 to 3
move 2 from 3 to 8
move 3 from 4 to 6
move 1 from 6 to 2
move 1 from 8 to 6
move 5 from 2 to 6
move 7 from 6 to 5
move 4 from 2 to 6
move 6 from 5 to 9
move 1 from 8 to 4
move 18 from 1 to 9
move 1 from 5 to 2
move 9 from 9 to 4
move 5 from 6 to 3
move 9 from 4 to 1
move 4 from 9 to 2
move 1 from 4 to 8
move 1 from 8 to 3
move 7 from 1 to 8
move 6 from 3 to 2
move 10 from 2 to 9
move 21 from 1 to 8
move 1 from 2 to 8
move 19 from 8 to 4
move 1 from 8 to 3
move 16 from 4 to 8
move 1 from 4 to 2
move 2 from 1 to 5
move 1 from 2 to 3
move 1 from 4 to 5
move 1 from 4 to 8
move 2 from 1 to 3
move 3 from 3 to 2
move 5 from 9 to 1
move 1 from 3 to 4
move 4 from 9 to 4
move 2 from 1 to 9
move 2 from 2 to 5
move 1 from 2 to 7
move 3 from 1 to 7
move 10 from 8 to 6
move 4 from 8 to 5
move 3 from 4 to 3
move 3 from 3 to 4
move 1 from 9 to 8
move 2 from 7 to 2
move 1 from 2 to 1
move 4 from 9 to 3
""";

#endregion


public class Crate
{
	public char Letter { get; }


	public Crate(char letter)
	{
		Letter = letter;
	}
}

public class CrateStack : Stack<Crate>
{
	public CrateStack(char[] letters) : base(letters.Select(l => new Crate(l)))
	{

	}
}

public enum CraneBehaviour {
	Single,
	Multiple
}

public class CrateMover
{
	public List<CrateStack> Stacks { get; }
	public IEnumerable<Instruction> Instructions { get; }
	
	public string TopCrates => string.Join(string.Empty, Stacks.Select(stack => stack.Peek().Letter));

	public CrateMover(List<CrateStack> stacks, IEnumerable<Instruction> instructions)
	{
		Instructions = instructions;
		Stacks = stacks;
	}

	public virtual void Operate()
	{
		foreach(var instruction in Instructions)
		{
			HandleInstruction(instruction);
		}
	}
	
	public virtual void HandleInstruction(Instruction instruction)
	{
		for (int i = 0; i < instruction.MoveCount; i++)
		{
			Stacks[instruction.ToStack].Push(Stacks[instruction.FromStack].Pop());
		}
	}
}

public class CrateMover9001 : CrateMover
{
	public CrateMover9001(List<CrateStack> stacks, IEnumerable<Instruction> instructions) : base(stacks, instructions)
	{	
		
	}

	public override void HandleInstruction(Instruction instruction)
	{
		var temp = new Stack<Crate>();

		for (int i = 0; i < instruction.MoveCount; i++)
		{			
			temp.Push(Stacks[instruction.FromStack].Pop());
		}

		for (int i = 0; i < instruction.MoveCount; i++)
		{
			Stacks[instruction.ToStack].Push(temp.Pop());
		}

		temp.Clear();
	}

}


public class Instruction
{
	public int MoveCount { get; }
	public int FromStack { get; }
	public int ToStack { get; }
	
	public Instruction(string instruction)
	{
		var segments = instruction.Split(' ', StringSplitOptions.TrimEntries);
		MoveCount = int.Parse(segments[1]);
		FromStack = int.Parse(segments[3]) - 1;
		ToStack = int.Parse(segments[5]) - 1 ;
	}
}
