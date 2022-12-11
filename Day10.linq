<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

public class Screen
{
	public int Cycle { get; set; }	
	public int SignalStrengthSum { get; private set; }	
	public int Register { get; private set; }
	
	private char LitPixel = '#';
	private char DarkPixel = '.';
	
	int[] cycles = new[] { 20, 60, 100, 140, 180, 220 };

	public char[,] Display = new char[6, 40];
	
	public Screen()
	{
		Cycle = 1;
		Register = 1;
		SignalStrengthSum = 0;
	}

	public void Process(string puzzleInput)
	{
		foreach (var instruction in puzzleInput.Split('\n', StringSplitOptions.RemoveEmptyEntries))
		{
			if (instruction[0..4] == "noop")
			{
				Spin(1);
			}
			else if (instruction[0..4] == "addx")
			{
				Spin(2);
				Register += int.Parse(instruction.Split(" ")[1]);
			}
		}
	}

	private void Tick()
	{
		if (cycles.Contains(Cycle))
		{
			SignalStrengthSum += Register * Cycle;
		}
		
		Draw();

		Cycle++;
	}

	private void Draw()
	{
		var x = (Cycle - 1) % 40;
		var y = (Cycle - 1) / 40;
		var isLitPixel = Math.Abs(x - Register) <= 1;		
		Display[y, x] = isLitPixel ? LitPixel : DarkPixel;
	}

	private void Spin(int cycles)
	{
		do
		{
			Tick();
			cycles--;
		}
		while (cycles > 0);
	}
}

async Task Main()
{
	var baseAddress = new Uri("https://adventofcode.com");
	var COOKIE = Util.GetPassword("adventofcode.cookie");

	using (var handler = new HttpClientHandler())
	using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
	{
		handler.CookieContainer.Add(baseAddress, new Cookie("session", COOKIE));

		var puzzleInput = await client.GetStringAsync("/2022/day/10/input");
		
		Screen screen = new Screen();		
		screen.Process(puzzleInput);		
		screen.Dump();
	}
}