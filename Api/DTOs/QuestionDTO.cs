using Api.Models;

namespace Api.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string? Text { get; set; }
        public string? OptionA { get; set; }
        public string? OptionB { get; set; }
        public string? OptionC { get; set; }
        public string? OptionD { get; set; }

        public QuestionDTO() { }
        public QuestionDTO(Question question) =>
        (Id, QuizId, Text, OptionA, OptionB, OptionC, OptionD) =
            (question.Id, question.QuizId, question.Text, question.OptionA, question.OptionB, question.OptionC, question.OptionD);
    }
}
