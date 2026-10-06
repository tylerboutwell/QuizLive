namespace Api.DTOs
{

    public class AnswerQuestionRequest
    {
        public required int PlayerId { get; set; }
        public required string Answer { get; set; }

        public AnswerQuestionRequest() { }
        public AnswerQuestionRequest(int playerId, string answer)
        {
            PlayerId = playerId;
            Answer = answer;
        }
    }
}