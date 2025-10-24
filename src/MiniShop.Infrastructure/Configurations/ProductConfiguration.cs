using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniShop.Domain.Products;

namespace MiniShop.Infrastructure.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("products");
        
        b.HasKey(p => p.Id);

        b.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        b.Property(p => p.Description)
            .HasMaxLength(2000);
        
        b.Property(p => p.Price)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        b.Property(p => p.Sku)
            .HasMaxLength(32)
            .IsRequired();

        b.Property(p => p.Stock)
            .IsRequired();
        
        b.Property(p => p.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        b.Property(p => p.CreatedAt)
            .IsRequired();
        
        b.Property(p => p.UpdatedAt)
            .IsRequired();
        
        b.HasIndex(p => p.Sku)
            .IsUnique()
            .HasDatabaseName("ux_products_sku");
       
        b.HasIndex(p => new { p.CreatedAt, p.Id })
            .HasDatabaseName("ix_products_created_at_id_live")
            .HasFilter("\"IsDeleted\" = FALSE");
        
        b.HasQueryFilter(p => !p.IsDeleted);
        
        b.Property(p => p.IsDeleted)
            .HasDefaultValue(false);
    }
}