namespace MiniShop.Application.Products.Dtos;

public sealed class ProductRequest
{
    public string Name { get; init; } = default!;
    public string Sku { get; init; } = default!;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public string? Description { get; init; }
}