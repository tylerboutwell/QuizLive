using Api.Endpoints;
using Api.Hubs;
using Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizLiveDb>(opt => opt.UseInMemoryDatabase("QuizLiveDb"));
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddScoped<GameService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

QuestionEndpoints.Map(app);
PlayerEndpoints.Map(app);
QuizEndpoints.Map(app);
GameEndpoints.Map(app);

app.MapHub<GameHub>("/gameHub");
app.Run();
