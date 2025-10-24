using MiniShop.Application.Products.Dtos;

namespace MiniShop.Application.Products;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(ProductRequest req,  CancellationToken ct);
    Task<ProductResponse?> GetAsync(Guid id, CancellationToken ct);
    Task<List<ProductResponse>> ListAsync(int page, int size, CancellationToken ct);
    Task<ProductResponse?> UpdateAsync(Guid id, ProductRequest req, CancellationToken ct);
    
    Task SetActiveAsync(Guid id, bool isActive, CancellationToken ct);
    Task SoftDeleteAsync(Guid id, string? by = null, string? reason = null, CancellationToken ct = default);
    Task RestoreAsync(Guid id, CancellationToken ct = default);
    Task HardDeleteAsync(Guid id, CancellationToken ct);
    
}