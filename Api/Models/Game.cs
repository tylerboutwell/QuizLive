namespace Api.Models
{
    public class Game
    {
        public int Id { get; set; }

        public int QuizId { get; set; }

        public string? GameCode { get; set; }

        public string? Status { get; set; }
    }
}
