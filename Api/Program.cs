using Microsoft.EntityFrameworkCore;
using Api.Endpoints;
using Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizLiveDb>(opt => opt.UseInMemoryDatabase("QuizLiveDb"));
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

QuestionEndpoints.Map(app);
PlayerEndpoints.Map(app);
QuizEndpoints.Map(app);
GameEndpoints.Map(app);

app.MapHub<GameHub>("/gameHub");
app.Run();
