using System.Data;
using FluentValidation;
using MiniShop.Application.Products.Dtos;

namespace MiniShop.Application.Products;

public sealed class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 100);

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("Sku is required")
            .Matches("^[A-Za-z0-9_-]{3,32}$").WithMessage("Sku must be 3..32 chars [A-Z0-9-_]");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be >= 0");
        
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock must be >= 0");

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}