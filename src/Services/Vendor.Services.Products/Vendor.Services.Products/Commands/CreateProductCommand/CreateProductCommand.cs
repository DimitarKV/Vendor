using AutoMapper;
using MediatR;
using Vendor.Domain.Entities;
using Vendor.Domain.Types;
using Vendor.Services.Products.Data.Persistence;
using Vendor.Services.Products.Views;

namespace Vendor.Services.Products.Commands.CreateProductCommand;

public class CreateProductCommand : IRequest<ApiResponse<ProductView>>
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductView>>
{
    private readonly ProductsDbContext _context;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(ProductsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //TODO: Add validation for product name already existing in the database
    public async Task<ApiResponse<ProductView>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product() { Name = request.Name, ImageUrl = request.ImageUrl };
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new ApiResponse<ProductView>(_mapper.Map<ProductView>(product), "Successfully created product");
    }
}