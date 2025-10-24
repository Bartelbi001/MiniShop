using MiniShop.Api.Extensions;
using MiniShop.Application;
using MiniShop.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration); 
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApiUi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();