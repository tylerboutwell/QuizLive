namespace Api.Endpoints
{
    public class Question
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/", async context =>
            {
                // Get all questions
                await context.Response.WriteAsJsonAsync(new { Message = "All todo items" });
            });

            app.MapGet("/{id}", async context =>
            {
                // Get one question
                await context.Response.WriteAsJsonAsync(new { Message = "One todo item" });
            });
        }
    }
}
