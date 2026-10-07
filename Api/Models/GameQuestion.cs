public class GameQuestion
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public int QuestionId { get; set; }
    public int Order { get; set; }

    public Game Game { get; set; } = null!;
    public Question Question { get; set; } = null!;
}