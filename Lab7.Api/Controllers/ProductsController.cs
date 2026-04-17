using Microsoft.AspNetCore.Mvc;

namespace Lab7.Api.Controllers;

public record Product(int Id, string Name, decimal Price);

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // Статичний список замість БД для простоти, але з симуляцією затримок
    private static readonly List<Product> _products = new()
    {
        new(1, "Widget", 9.99m),
        new(2, "Gadget", 24.99m),
        new(3, "Doohickey", 4.99m),
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        await Task.Delay(50); // Симуляція затримки виклику до БД
        return Ok(_products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        await Task.Delay(20); // Швидший запит, бо пошук по ID
        var product = _products.FirstOrDefault(p => p.Id == id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        await Task.Delay(30); // Симуляція запису в БД
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        await Task.Delay(200); // Симуляція важких обчислень або складного JOIN в БД
        
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Search term is required.");

        var results = _products.Where(p =>
            p.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
        
        return Ok(results);
    }
}