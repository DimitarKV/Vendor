using AutoMapper;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Services.Products.Data.Persistence;
using Vendor.Services.Products.Views;

namespace Vendor.Services.Products.Queries.QueryProductById;

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

    // TODO: Add validations for missing id in database
    public async Task<ApiResponse<ProductView>> Handle(QueryProductById request, CancellationToken cancellationToken)
    {
        return new ApiResponse<ProductView>(
            _mapper.Map<ProductView>(
                await _context.Products.FindAsync(request.Id, cancellationToken)
                ), "Successfully queried product from database"
            );
    }
}