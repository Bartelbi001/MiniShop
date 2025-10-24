using Microsoft.EntityFrameworkCore;
using MiniShop.Application.Products;
using MiniShop.Domain.Products;
using MiniShop.Infrastructure.Persistence;

namespace MiniShop.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly MiniShopDbContext _db;
    public ProductRepository(MiniShopDbContext db) => _db = db;
    
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) 
        => _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Product?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken ct)
        => _db.Products
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    
    public Task<Product?> GetBySkuAsync(string sku, CancellationToken ct) 
        => _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Sku == sku, ct);

    public Task<List<Product>> ListAsync(int skip, int take, CancellationToken ct) 
        => _db.Products
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ThenByDescending(p => p.Id)
            .Skip(skip)
            .Take(take)
            .AsNoTracking()
            .ToListAsync(ct);

    public Task AddAsync(Product product, CancellationToken ct)
        => _db.Products.AddAsync(product, ct).AsTask();

    public void Remove(Product product) => _db.Products.Remove(product);
    
    public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}