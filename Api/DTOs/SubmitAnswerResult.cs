using Api.Models;

public class SubmitAnswerResult
{
    public bool IsCorrect { get; set; }
    public List<Player> Players { get; set; } = [];
}