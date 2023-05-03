using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Queries;

public class QueryMissingProducts : IRequest<ApiResponse<List<int>>>
{
    public int MachineId { get; set; }
}

public class QueryMissingProductsHandler : IRequestHandler<QueryMissingProducts, ApiResponse<List<int>>>
{
    private readonly IMachineRepository _repository;

    public QueryMissingProductsHandler(IMachineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<List<int>>> Handle(QueryMissingProducts request, CancellationToken cancellationToken)
    {
        var machine = await _repository.GetVendingAndSpiralsByIdAsync(request.MachineId);
        var productsQuantities = new Dictionary<int, int>();
        foreach (var spiral in machine.Spirals)
        {
            if(spiral.ProductId == -1)
                continue;
            if (productsQuantities.ContainsKey(spiral.ProductId))
                productsQuantities[spiral.ProductId] += spiral.Loads;
            else
                productsQuantities[spiral.ProductId] = spiral.Loads;
        }

        var missingProducts = productsQuantities.Where(p => p.Value == 0).Select(p => p.Key).ToList();
        
        return new ApiResponse<List<int>>(missingProducts);
    }
}

public class QueryMissingProductsValidator : AbstractValidator<QueryMissingProducts>
{
    public QueryMissingProductsValidator(IMachineRepository repository)
    {
        RuleFor(q => q)
            .MustAsync(async (q, _) => (await repository.GetVendingByIdAsync(q.MachineId)) is not null)
            .WithMessage("No such machine in the database!")
            .WithErrorCode("400");
    }
}