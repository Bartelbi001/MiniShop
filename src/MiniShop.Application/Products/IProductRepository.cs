using MiniShop.Domain.Products;

namespace MiniShop.Application.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Product?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken ct);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken ct);
    Task<List<Product>> ListAsync(int skip, int take, CancellationToken ct);
    Task AddAsync(Product product, CancellationToken ct);
    void Remove(Product product);
    Task<int> SaveChangesAsync(CancellationToken ct);
}