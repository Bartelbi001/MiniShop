namespace MiniShop.Application.Products.Dtos;

public sealed class ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string Sku { get; init; } = default!;
    public int Stock { get; init; }
    public bool IsActive { get; init; }
    
    public bool IsDeleted { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
    public string? DeleteReason { get; init; }
    
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}