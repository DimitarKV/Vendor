using AutoMapper;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Queries;

public class QueryEmptyVendings : IRequest<ApiResponse<List<VendingView>>> { }

public class QueryEmptyVendingsHandler : IRequestHandler<QueryEmptyVendings, ApiResponse<List<VendingView>>>
{
    private readonly IMapper _mapper;
    private readonly IMachineRepository _repository;

    public QueryEmptyVendingsHandler(IMapper mapper, IMachineRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResponse<List<VendingView>>> Handle(QueryEmptyVendings request, CancellationToken cancellationToken)
    {
        var emptyVendings = await _repository.GetEmptyVendingsAsync();
        var emptyVendingsViews = emptyVendings.Select(ev => _mapper.Map<VendingView>(ev)).ToList();
        
        return new ApiResponse<List<VendingView>>(emptyVendingsViews, "Successfully queried all empty vendings!");
    }
}