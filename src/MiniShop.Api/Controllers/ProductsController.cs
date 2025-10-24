using Microsoft.AspNetCore.Mvc;
using MiniShop.Application.Products;
using MiniShop.Application.Products.Dtos;

namespace MiniShop.Api.Controllers;

[ApiController]
[Route("api/products")]
[Produces("application/json")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    
    public ProductsController(IProductService service) => _service = service;

    /// <summary>Get paged list of products.</summary>
    /// <param name="page">Page number (>= 1).</param>
    /// <param name="size">Page size (1..100).</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductResponse>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int size = 20,
        CancellationToken ct = default)
    {
        var items = await _service.ListAsync(page, size, ct);
        return Ok(items);
    }

    /// <summary>Get product by id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> Get(Guid id, CancellationToken ct)
    {
        var result = await _service.GetAsync(id, ct);
        return Ok(result);
    }

    /// <summary>Create a new product.</summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProductResponse>> Create(
        [FromBody] ProductRequest request,
        CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    /// <summary>Update an existing product.</summary>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> Update(
        Guid id,
        [FromBody] ProductRequest request,
        CancellationToken ct)
    {
        var updated = await _service.UpdateAsync(id, request, ct);
        return Ok(updated);
    }

    /// <summary>Activate/Deactivate a product.</summary>
    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType((StatusCodes.Status404NotFound))]
    public async Task<IActionResult> SetActivate(
        Guid id,
        [FromQuery] bool active,
        CancellationToken ct)
    {
        await _service.SetActiveAsync(id, active, ct);
        return NoContent();
    }
    
    /// <summary>Delete a product(hard or soft).</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] bool hard = false,
        CancellationToken ct  = default)
    {
        if (hard)
        {
            await _service.HardDeleteAsync(id, ct);
            return NoContent();
        }
        
        await _service.SoftDeleteAsync(id, by: "system", reason: null, ct);
        return NoContent();
    }
    
    /// <summary>Restore a product.</summary>
    [HttpPost("{id:guid}/restore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Restore(Guid id, CancellationToken ct = default)
    {
        await _service.RestoreAsync(id, ct);
        return Ok();
    }
}