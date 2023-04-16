using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;

namespace Vendor.Services.Products.Api.CQRS.Queries;

public class QueryProductById : IRequest<ApiResponse<ProductView>>
{
    public int Id { get; set; }
}

public class QueryProductByIdHandler : IRequestHandler<QueryProductById, ApiResponse<ProductView>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public QueryProductByIdHandler(IMapper mapper, IProductRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResponse<ProductView>> Handle(QueryProductById request, CancellationToken cancellationToken)
    {
        return new ApiResponse<ProductView>(
            _mapper.Map<ProductView>(
                await _repository.FindProductByIdAsync(request.Id)
            ), "Successfully queried product from database"
        );
    }
}

public class QueryProductByIdValidator : AbstractValidator<QueryProductById>
{
    public QueryProductByIdValidator(IProductRepository repository)
    {
        RuleFor(q => q.Id)
            .MustAsync(async (id, _) => await repository.FindProductByIdAsync(id) is not null)
            .WithMessage("No such product with the given id in the database!")
            .WithErrorCode("409");
    }
}