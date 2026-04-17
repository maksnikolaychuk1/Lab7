using Lab7.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Налаштування PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

// Автоматичне створення бази та наповнення даними
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Students.Any())
    {
        var students = Enumerable.Range(1, 10000).Select(i => new Student
        {
            Name = $"Student {i}",
            Email = $"student{i}@example.com"
        }).ToList();

        db.Students.AddRange(students);
        db.SaveChanges();
    }
}

app.MapControllers();
app.Run();