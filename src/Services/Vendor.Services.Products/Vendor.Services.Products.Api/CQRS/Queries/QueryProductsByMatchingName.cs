using AutoMapper;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;

namespace Vendor.Services.Products.Api.CQRS.Queries;

public class QueryProductsByMatchingName : IRequest<ApiResponse<List<ProductView>>>
{
    public string Name { get; set; }

    public QueryProductsByMatchingName(string name)
    {
        Name = name;
    }
}

public class
    QueryProductsByMatchingNameHandler : IRequestHandler<QueryProductsByMatchingName, ApiResponse<List<ProductView>>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public QueryProductsByMatchingNameHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductView>>> Handle(QueryProductsByMatchingName request,
        CancellationToken cancellationToken)
    {
        var products = await _repository.FindProductsByMatchingNameAsync(request.Name);
        var productsMapped = products.Select(p => _mapper.Map<ProductView>(p)).ToList();
        return new ApiResponse<List<ProductView>>(productsMapped, "Successfully queried matching products");
    }
}