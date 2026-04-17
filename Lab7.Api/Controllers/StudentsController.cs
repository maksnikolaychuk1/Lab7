using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab7.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Повертаємо всіх студентів (це створить навантаження на мережу та пам'ять)
        var students = await _context.Students.ToListAsync();
        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _context.Students.FindAsync(id);
        return student is null ? NotFound() : Ok(student);
    }
}