namespace Api.DTOs
{

    public class CreateGameRequest
    {
        public required int QuizId { get; set; }
        public required string PlayerName { get; set; }

        public CreateGameRequest() { }
        public CreateGameRequest(int quizId, string playerName)
        {
            QuizId = quizId;
            PlayerName = playerName;
        }
    }
}