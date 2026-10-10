using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TodoApi.Data;
using TodoApi.Middleware;
using TodoApi.Repository;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
