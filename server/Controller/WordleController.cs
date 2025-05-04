using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class WordleController : ControllerBase
{
    private static readonly List<string> wordList = System.IO.File.ReadAllLines("Data/WordList.txt")
        .Where(w => w.Length == 5)
        .Select(w => w.Trim().ToLower())
        .ToList();

    private static readonly Random random = new();

    [HttpGet("word")]
    public IActionResult GetRandomWord()
    {
        var word = wordList[random.Next(wordList.Count)];
        return Ok(new { word });
    }

    public record GuessRequest(string Guess, string Target);

    // It is inefficient to make HTTP call to validate every guess
    [HttpPost("validate")]
    public IActionResult ValidateGuess([FromBody] GuessRequest request)
    {
        string guess = request.Guess.ToLower();
        // No need to convert target to lowercase, word list will be cleaned to only contain lowercase
        string target = request.Target.ToLower();
        var result = new List<string>();

        for (int i = 0; i < 5; i++)
        {
            if (guess[i] == target[i])
                result.Add("green");
            else if (target.Contains(guess[i]))
                result.Add("yellow");
            else
                result.Add("gray");
        }

        return Ok(new { result });
    }
}