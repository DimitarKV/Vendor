using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Entities;
using Vendor.Domain.Types;
using Vendor.Services.Products.Data.Persistence;
using Vendor.Services.Products.Views;

namespace Vendor.Services.Products.Commands;

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
    
    public async Task<ApiResponse<ProductView>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product() { Name = request.Name, ImageUrl = request.ImageUrl };
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new ApiResponse<ProductView>(_mapper.Map<ProductView>(product), "Successfully created product");
    }
}

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