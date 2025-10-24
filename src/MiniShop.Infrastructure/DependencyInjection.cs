using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Application.Products;
using MiniShop.Infrastructure.Persistence;
using MiniShop.Infrastructure.Repositories;

namespace MiniShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("DefaultConnection") 
                 ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<MiniShopDbContext>(opt =>
        {
            opt.UseNpgsql(cs);
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        
        return services;
    }
}