namespace Api.DTOs
{

    public class CreateGameRequest
    {
        public required int QuizId { get; set; }

        public CreateGameRequest() { }
        public CreateGameRequest(int quizId)
        {
            QuizId = quizId;
        }
    }
}