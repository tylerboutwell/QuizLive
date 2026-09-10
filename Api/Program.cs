using Microsoft.EntityFrameworkCore;
using Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizLiveDb>(opt => opt.UseInMemoryDatabase("QuizLiveDb"));
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

QuestionEndpoints.Map(app);

app.Run();
