using MiniShop.Domain.Abstractions;
using MiniShop.Domain.Common;

namespace MiniShop.Domain.Products;

public sealed class Product : Entity
{
    public string Name { get; private set; }
    public string? Description { get;  private set; }
    public decimal Price { get;  private set; }
    public string Sku { get;  private set; }
    public int Stock { get;  private set; }
    public bool IsActive { get;  private set; }
    
    // Soft-delete
    public bool IsDeleted { get;  private set; }
    public DateTimeOffset? DeletedAt { get;  private set; }
    public string? DeletedBy { get;  private set; }
    public string? DeleteReason { get;  private set; }
    
    private Product() { }

    private Product(Guid id, string name, string sku, decimal price, int stock, string? description, DateTimeOffset nowUtc)
    {
        Id = id;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(Name), 2, 100);
        Sku = sku;
        Price = Guard.NonNegativeMoney(price, nameof(Price));
        Stock = Guard.NonNegative(stock, nameof(Stock));
        Description = description;
        IsActive = true;
        CreatedAt = nowUtc;
        UpdatedAt = nowUtc;

        Validate();
    }
    
    public static Product Create(string name, string sku, decimal price, int stock, string? description = null)
        => Create(name, sku, price, stock, DateTimeOffset.UtcNow, description);
    
    public static Product Create(string name, string sku, decimal price, int stock, DateTimeOffset nowUtc, string? description = null)
        => new(Guid.NewGuid(), name, sku, price, stock, description, nowUtc);

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        Validate();
        Touch();
    }

    public void ChangePrice(decimal price)
    {
        Price = NormalizePrice(price);
        Touch();
    }

    public void AdjustStock(int delta)
    {
        if(Stock + delta < 0)
            throw new DomainValidationException("Stock cannot be negative.");
        Stock += delta;
        Touch();
    }
    
    public void Activate() { if (!IsActive) { IsActive = true; Touch(); } }
    public void Deactivate() { if (IsActive) { IsActive = false; Touch(); } }
    
    // --- Soft delete API ---
    public void Delete(string? by = null, string? reason = null)
    {
        if (IsDeleted) return;
        
        IsDeleted = true;
        DeletedBy = by;
        DeleteReason = reason;
        
        var now = DateTimeOffset.UtcNow;
        DeletedAt = now;
        UpdatedAt = now;
    }

    public void Restore()
    {
        if(!IsDeleted) return;
        
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        DeleteReason = null;
        
        Touch();
    }
    // -----------------------

    private static decimal NormalizePrice(decimal value)
    {
        if (value < 0) throw new DomainValidationException("Price must be >= 0.");
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
    
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name) || Name.Length is < 2 or > 100)
            throw new DomainValidationException("Name must be 2..100 chars.");
        
        if (Description is { Length: > 2000})
            throw new DomainValidationException("Description length must be <= 2000.");
        
        if (string.IsNullOrWhiteSpace(Sku) || Sku.Length is < 3 or > 32 || !Sku.All(c => char.IsLetterOrDigit(c) || c is '-' or '_'))
            throw new DomainValidationException("Sku must be 3..32 chars [A-Z0-9-_].");
        
        if (Stock < 0) throw new DomainValidationException("Stock must be >= 0.");
    }
    
    private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}