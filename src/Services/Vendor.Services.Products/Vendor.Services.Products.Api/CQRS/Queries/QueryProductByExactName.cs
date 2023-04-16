using MediatR;
using Vendor.Domain.Types;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;

namespace Vendor.Services.Products.Api.CQRS.Queries;

public class QueryProductByExactName : IRequest<ApiResponse<Product?>>
{
    public string Name { get; set; }

    public QueryProductByExactName(string name)
    {
        Name = name;
    }
}

public class QueryProductByExactNameHandler : IRequestHandler<QueryProductByExactName, ApiResponse<Product?>>
{
    private readonly IProductRepository _repository;

    public QueryProductByExactNameHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<Product?>> Handle(QueryProductByExactName request,
        CancellationToken cancellationToken)
    {
        return new ApiResponse<Product?>(await _repository.FindProductByExactNameAsync(request.Name), "Product exists!");
    }
}