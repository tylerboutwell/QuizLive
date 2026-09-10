using Api.Models;
using Microsoft.EntityFrameworkCore;

class QuizLiveDb : DbContext
{
    public QuizLiveDb(DbContextOptions<QuizLiveDb> options)
        : base(options) { }

    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Game> Games => Set<Game>();
}