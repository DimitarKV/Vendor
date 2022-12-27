using FluentValidation;
using Vendor.Services.Products.Data.Persistence;

namespace Vendor.Services.Products.Queries.QueryProductById;

public class QueryProductByIdValidator : AbstractValidator<QueryProductById>
{
    public QueryProductByIdValidator(ProductsDbContext _context)
    {
        RuleFor(q => q.Id)
            .MustAsync(async (id, _) =>
            {
                var product = await _context.Products.FindAsync(id);
                if (product is null)
                    return false;
                return true;
            })
            .WithMessage("No such product with the given id in the database!")
            .WithErrorCode("409");
    }
}