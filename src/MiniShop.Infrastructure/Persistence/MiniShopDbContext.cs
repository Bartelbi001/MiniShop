using Microsoft.EntityFrameworkCore;
using MiniShop.Domain.Products;
using MiniShop.Infrastructure.Configurations;

namespace MiniShop.Infrastructure.Persistence;

public sealed class MiniShopDbContext : DbContext
{
    public MiniShopDbContext(DbContextOptions<MiniShopDbContext> options) : base(options) { }
    
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}