using MiniShop.Application.Common;
using MiniShop.Application.Products.Mapping;
using MiniShop.Application.Products.Dtos;
using MiniShop.Domain.Products;

namespace MiniShop.Application.Products;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    
    public ProductService(IProductRepository repo) => _repo = repo;
    
    public async Task<ProductResponse> CreateAsync(ProductRequest req, CancellationToken ct)
    {
        if (await _repo.GetBySkuAsync(req.Sku, ct) is not null)
            throw new ConflictException($"SKU '{req.Sku}' already exists.", propertyName: nameof(req.Sku));

        var now = DateTimeOffset.UtcNow;
        var p = Product.Create(req.Name, req.Sku, req.Price, req.Stock, now, req.Description);

        await _repo.AddAsync(p, ct);
        await _repo.SaveChangesAsync(ct);
        
        return ProductMapper.ToPublicResponse(p);
    }

    public async Task<ProductResponse?> GetAsync(Guid id, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.", "Product", id.ToString());

        return ProductMapper.ToPublicResponse(p);
    }

    public async Task<List<ProductResponse>> ListAsync(int page, int size, CancellationToken ct)
    {
        page = Math.Max(1, page);
        size = Math.Clamp(size, 1, 100);
        
        var items = await _repo.ListAsync((page - 1) * size, size, ct);
        
        return items.Select(ProductMapper.ToPublicResponse).ToList();
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, ProductRequest req, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.", "Product", id.ToString());

        p.UpdateDetails(req.Name, req.Description);
        p.ChangePrice(req.Price);

        var delta = req.Stock - p.Stock;
        if (delta != 0) p.AdjustStock(delta);
        
        await _repo.SaveChangesAsync(ct);
        
        return ProductMapper.ToPublicResponse(p);
    }

    public async Task SetActiveAsync(Guid id, bool isActive, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.", "Product", id.ToString());

        if (isActive) p.Activate(); else p.Deactivate();
        
        await _repo.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(Guid id, string? by = null, string? reason = null, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.", "Product", id.ToString());
        
        if (p.IsDeleted) return;
        
        p.Delete(by, reason);
        await _repo.SaveChangesAsync(ct);
    }

    public async Task RestoreAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdIncludingDeletedAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.", "Product", id.ToString());
        
        if (!p.IsDeleted) return;
        
        p.Restore();
        await _repo.SaveChangesAsync(ct);
    }

    public async Task HardDeleteAsync(Guid id, CancellationToken ct)
    {
        var p = await _repo.GetByIdIncludingDeletedAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.", "Product", id.ToString());
        
        _repo.Remove(p);
        await _repo.SaveChangesAsync(ct);
    }
}