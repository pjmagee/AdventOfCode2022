<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

var baseAddress = new Uri("https://adventofcode.com");
var COOKIE = Util.GetPassword("adventofcode.cookie");

using (var handler = new HttpClientHandler())
using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
{
	handler.CookieContainer.Add(baseAddress, new Cookie("session", COOKIE));
	var puzzleInput = await client.GetStringAsync("/2022/day/2/input");

	puzzleInput
		.Split('\n', StringSplitOptions.RemoveEmptyEntries)
		.Select(roundText =>
		{
			var choices = roundText.Split(' ');
			var opponent = Decipher.Map(choices[0]);
			var me = Decipher.Part2Map(opponent, choices[1]);
			return new Round(opponent, me);
		})
		.Sum(round => round.CalculateScore())
		.Dump();
}

public class Round
{
	public Choice Opponent { get; set; }
	public Choice Me { get; set; }

	public const int WinPoints = 6;
	public const int DrawPoints = 3;
	public const int LossPoints = 0;

	public Round(Choice opponent, Choice me)
	{
		Opponent = opponent;
		Me = me;
	}

	public int CalculateScore()
	{
		if (Opponent.IsRock() && Me.IsPaper()) return ((int)Me) + WinPoints;
		if (Opponent.IsPaper() && Me.IsScissors()) return ((int)Me) + WinPoints;
		if (Opponent.IsScissors() && Me.IsRock()) return ((int)Me) + WinPoints;
		if (Opponent == Me) return ((int)Me) + DrawPoints;

		return ((int)Me) + LossPoints;
	}
}

public static class EnumExtensions
{
	public static bool IsRock(this Choice choice) => choice == Choice.Rock;
	public static bool IsPaper(this Choice choice) => choice == Choice.Paper;
	public static bool IsScissors(this Choice choice) => choice == Choice.Scissors;

	public static Choice CalculateChoiceToLose(this Choice choice)
	{
		if (choice is Choice.Scissors) return Choice.Paper;
		if (choice is Choice.Rock) return Choice.Scissors;
		if (choice is Choice.Paper) return Choice.Rock;
		throw new Exception("Unhandled Choice");
	}

	public static Choice CalculateChoiceForDraw(this Choice choice) => choice;

	public static Choice CalculateChoiceToWin(this Choice choice)
	{
		if (choice is Choice.Scissors) return Choice.Rock;
		if (choice is Choice.Rock) return Choice.Paper;
		if (choice is Choice.Paper) return Choice.Scissors;
		throw new Exception("Unhandled Choice");
	}
}

public enum Choice
{
	Rock = 1,
	Paper = 2,
	Scissors = 3
}

public class Decipher
{
	public static Choice Map(string value)
	{
		return value switch
		{
			"A" or "X" => Choice.Rock,
			"B" or "Y" => Choice.Paper,
			"C" or "Z" => Choice.Scissors
		};
	}

	public static Choice Part2Map(Choice opponent, string value)
	{
		if (ShouldLose(value)) return opponent.CalculateChoiceToLose();
		if (ShouldWin(value)) return opponent.CalculateChoiceToWin();
		if (ShouldDraw(value)) return opponent.CalculateChoiceForDraw();
		throw new Exception("Unhandled value");
	}

	private static bool ShouldLose(string value) => value is "X";
	private static bool ShouldWin(string value) => value is "Z";
	private static bool ShouldDraw(string value) => value is "Y";
}