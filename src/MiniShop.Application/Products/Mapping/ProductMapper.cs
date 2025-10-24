using MiniShop.Application.Products.Dtos;
using MiniShop.Domain.Products;

namespace MiniShop.Application.Products.Mapping;

public static class ProductMapper
{
    public static ProductResponse ToPublicResponse(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Sku = p.Sku,
        Stock = p.Stock,
        IsActive = p.IsActive,
        IsDeleted = p.IsDeleted,
        DeletedAt = p.DeletedAt,
        DeletedBy = p.DeletedBy,
        DeleteReason = p.DeleteReason,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
    };

    public static ProductAdminResponse ToAdminResponse(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Sku = p.Sku,
        Stock = p.Stock,
        IsActive = p.IsActive,
        IsDeleted = p.IsDeleted,
        DeletedAt = p.DeletedAt,
        DeletedBy = p.DeletedBy,
        DeleteReason = p.DeleteReason,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
    };
}