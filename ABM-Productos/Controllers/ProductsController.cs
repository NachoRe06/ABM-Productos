using Microsoft.AspNetCore.Mvc;
using ABM_Productos.Models.DTOs.Requests;
using ABM_Productos.Models.DTOs.Responses;
using ABM_Productos.Services.Interfaces;

namespace ABM_Productos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<ProductForReadDto>> GetAll()
    {
        var products = _service.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductForReadDto> GetById(int id)
    {
        var product = _service.GetProductById(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public ActionResult<ProductForReadDto> Create([FromBody] ProductForCreateDto dto)
    {
        try
        {
            var createdProduct = _service.CreateProduct(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductForUpdateDto dto)
    {
        var existing = _service.GetProductById(id);

        if (existing is null)
        {
            return NotFound();
        }

        try
        {
            _service.UpdateProduct(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _service.GetProductById(id);

        if (existing is null)
        {
            return NotFound();
        }

        _service.DeleteProduct(id);
        return NoContent();
    }

    [HttpGet("search")]
    public ActionResult<List<ProductForReadDto>> SearchByName([FromQuery] string? name)
    {
        var results = _service.SearchProductsByName(name ?? string.Empty);
        return Ok(results);
    }

    [HttpGet("stats")]
    public ActionResult<ProductStatsDto> GetStats()
    {
        var stats = _service.GetStats();
        return Ok(stats);
    }
}