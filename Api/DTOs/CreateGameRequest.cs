namespace Api.DTOs
{

    public class JoinGameRequest
    {
        public required int QuizId { get; set; }

        public JoinGameRequest() { }
        public JoinGameRequest(int quizId)
        {
            QuizId = quizId;
        }
    }
}