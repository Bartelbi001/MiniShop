using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Application.Common;
using MiniShop.Domain.Common;

namespace MiniShop.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainValidationException ex)
        {
            await WriteProblem(context, ex.Message, StatusCodes.Status400BadRequest, "domain_validation");
        }
        catch (ConflictException ex)
        {
            await WriteProblem(context, ex.Message, StatusCodes.Status409Conflict, "conflict");
        }
        catch (NotFoundException ex)
        {
            await WriteProblem(context, ex.Message, StatusCodes.Status404NotFound, "not found");
        }
        catch (Exception ex)
        {
            await WriteProblem(context, "Internal server error", StatusCodes.Status500InternalServerError, "unexpected_error");
        }
    }

    private static async Task WriteProblem(HttpContext ctx, string detail, int status, string type)
    {
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = status;

        var problem = new ProblemDetails
        {
            Title = type,
            Detail = detail,
            Status = status,
            Type = $"urn:problem-type:{type}",
            Instance = ctx.TraceIdentifier
        };

        await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}