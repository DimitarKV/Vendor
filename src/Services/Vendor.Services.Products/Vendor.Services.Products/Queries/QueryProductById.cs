using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Products.Data.Persistence;

namespace Vendor.Services.Products.Queries;

public class QueryProductById : IRequest<ApiResponse<ProductView>>
{
    public int Id { get; set; }
}

public class QueryProductByIdHandler : IRequestHandler<QueryProductById, ApiResponse<ProductView>>
{
    private readonly ProductsDbContext _context;
    private readonly IMapper _mapper;

    public QueryProductByIdHandler(ProductsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductView>> Handle(QueryProductById request, CancellationToken cancellationToken)
    {
        return new ApiResponse<ProductView>(
            _mapper.Map<ProductView>(
                await _context.Products.FindAsync(request.Id, cancellationToken)
                ), "Successfully queried product from database"
            );
    }
}

public class QueryProductByIdValidator : AbstractValidator<QueryProductById>
{
    public QueryProductByIdValidator(ProductsDbContext context)
    {
        RuleFor(q => q.Id)
            .MustAsync(async (id, _) =>
            {
                var product = await context.Products.FindAsync(id);
                if (product is null)
                    return false;
                return true;
            })
            .WithMessage("No such product with the given id in the database!")
            .WithErrorCode("409");
    }
}