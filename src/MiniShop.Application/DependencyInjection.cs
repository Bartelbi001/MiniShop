using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Application.Products;

namespace MiniShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}