using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace MiniShop.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddControllers();
        
        services.AddProblemDetails();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniShop API", Version = "v1" });
        });
        
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(MiniShop.Application.DependencyInjection).Assembly);

        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ReportApiVersions = true;
        });

        services.AddHealthChecks();

        services.AddCors(opt =>
        {
            opt.AddPolicy("Default", policy =>
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:5173"));
        });
        
        return services;
    }
}