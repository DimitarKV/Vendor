using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;

namespace Vendor.Services.Products.Api.CQRS.Commands;

public class CreateProductCommand : IRequest<ApiResponse<ProductView>>
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductView>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<ProductView>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.ImageUrl);
        _repository.AddProduct(product);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiResponse<ProductView>(_mapper.Map<ProductView>(product), "Successfully created product");
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductRepository repository)
    {
        RuleFor(p => p.Name)
            .MustAsync(async (name, _) =>
                (await repository.FindProductByExactNameAsync(name)) is null)
            .WithMessage("Product name already exists in database!")
            .WithErrorCode("409");
    }
}