using Api.Models;


namespace Api.Data
{
    public static class SeedData
    {

        public static void Seed(QuizLiveDb db)
        {
            var sports = new Quiz
            {
                Title = "Sports",
                Description = "Test your knowledge of sports!"
            };

            var movies = new Quiz
            {
                Title = "Movies",
                Description = "Test your knowledge of movie!"
            };

            var music = new Quiz
            {
                Title = "Music",
                Description = "Test your knowledge of music!"
            };

            db.Quizzes.AddRange(sports, movies, music);
            db.SaveChanges();

            /// Add questions to the sports quiz
            var sportsQuestions = new List<Question>
            {
                new Question
                {
                    Text = "Which country won the FIFA World Cup in 2018?",
                    QuizId = sports.Id,
                    OptionA = "Germany",
                    OptionB = "Brazil",
                    OptionC = "France",
                    OptionD = "Argentina",
                    CorrectOption = "C"
                },
                new Question
                {
                    Text = "Who holds the record for the most home runs in a single MLB season?",
                    QuizId = sports.Id,
                    OptionA = "Barry Bonds",
                    OptionB = "Babe Ruth",
                    OptionC = "Mark McGwire",
                    OptionD = "Sammy Sosa",
                    CorrectOption = "A"
                }
            };

            /// Add questions to the movies quiz
            var moviesQuestions = new List<Question>
            {
                new Question
                {
                    Text = "Who directed the movie 'Inception'?",
                    QuizId = movies.Id,
                    OptionA = "Christopher Nolan",
                    OptionB = "Steven Spielberg",
                    OptionC = "James Cameron",
                    OptionD = "Quentin Tarantino",
                    CorrectOption = "A"
                },
                new Question
                {
                    Text = "Which movie won the Academy Award for Best Picture in 2020?",
                    QuizId = movies.Id,
                    OptionA = "1917",
                    OptionB = "Joker",
                    OptionC = "Parasite",
                    OptionD = "Once Upon a Time in Hollywood",
                    CorrectOption = "C"
                }
            };

            /// Add questions to the music quiz
            var musicQuestions = new List<Question>
            {
                new Question
                {
                    Text = "Who is known as the 'King of Pop'?",
                    QuizId = music.Id,
                    OptionA = "Elvis Presley",
                    OptionB = "Michael Jackson",
                    OptionC = "Prince",
                    OptionD = "Madonna",
                    CorrectOption = "B"
                },
                new Question
                {
                    Text = "Which band released the album 'Abbey Road'?",
                    QuizId = music.Id,
                    OptionA = "The Beatles",
                    OptionB = "The Rolling Stones",
                    OptionC = "Pink Floyd",
                    OptionD = "Led Zeppelin",
                    CorrectOption = "A"
                }
            };


            db.Questions.AddRange(sportsQuestions);
            db.Questions.AddRange(moviesQuestions);
            db.Questions.AddRange(musicQuestions);
            db.SaveChanges();
        }
    }
}