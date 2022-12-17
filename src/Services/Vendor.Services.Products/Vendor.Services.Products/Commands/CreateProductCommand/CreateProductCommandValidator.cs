using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vendor.Services.Products.Data.Persistence;

namespace Vendor.Services.Products.Commands.CreateProductCommand;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(ProductsDbContext context)
    {
        RuleFor(p => p.Name)
            .MustAsync(async (name, _) =>
                await context.Products.Where(p => p.Name == name).FirstOrDefaultAsync() is null)
            .WithMessage("Product name already exists in database!")
            .WithErrorCode("409");
    }
}