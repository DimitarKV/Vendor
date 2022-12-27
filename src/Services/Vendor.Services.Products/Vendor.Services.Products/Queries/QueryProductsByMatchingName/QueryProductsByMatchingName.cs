using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Services.Products.Data.Persistence;
using Vendor.Services.Products.Views;

namespace Vendor.Services.Products.Queries.QueryProductsByMatchingName;

public class QueryProductsByMatchingName : IRequest<ApiResponse<List<ProductView>>>
{
    public string Name { get; set; }
}

public class
    QueryProductsByMatchingNameHandler : IRequestHandler<QueryProductsByMatchingName, ApiResponse<List<ProductView>>>
{
    private readonly ProductsDbContext _context;
    private readonly IMapper _mapper;

    public QueryProductsByMatchingNameHandler(ProductsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductView>>> Handle(QueryProductsByMatchingName request,
        CancellationToken cancellationToken)
    {
        var products = await _context.Products.Where(p => p.Name.Contains(request.Name))
            .ToListAsync(cancellationToken);
        var productsMapped = products.Select(p => _mapper.Map<ProductView>(p)).ToList();
        return new ApiResponse<List<ProductView>>(productsMapped, "Successfully queried matching products");
    }
}