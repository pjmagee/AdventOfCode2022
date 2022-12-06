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

		var puzzleInput = (await client.GetStringAsync("/2022/day/6/input")).ToArray();

		HandheldDevice.GetStartOfPacket(puzzleInput).Dump("Part 1");
		HandheldDevice.GetStartOfMessage(puzzleInput).Dump("Part 2");
	}
}


public class HandheldDevice
{
	public static int GetStartOf(char[] buffer, int size)
	{
		foreach(var i in Enumerable.Range(0, buffer.Length))
		{				
			if (buffer[i..(i + size)].ToHashSet().Count == size)
			{
				return i + size;
			}
		}

		throw new Exception($"Could not find marker of size: {size}"); 
	}

	public static int GetStartOfMessage(char[] buffer) => GetStartOf(buffer[0..^1], 14);

	public static int GetStartOfPacket(char[] buffer) => GetStartOf(buffer[0..^1], 4);
}
