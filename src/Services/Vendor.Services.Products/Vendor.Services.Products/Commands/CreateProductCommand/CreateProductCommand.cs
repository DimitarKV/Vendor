using MediatR;
using Vendor.Domain.Types;

namespace Vendor.Services.Products.Commands.CreateProductCommand;

public class CreateProductCommand : IRequest<ApiResponse<int>>
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<int>>
{
    public Task<ApiResponse<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}