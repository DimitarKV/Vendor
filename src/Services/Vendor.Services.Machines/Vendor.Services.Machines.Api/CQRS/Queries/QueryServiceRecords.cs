using MediatR;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Queries;

public class QueryServiceRecords : IRequest<ApiResponse<List<Servicing>>>
{
    public int MachineId { get; set; }
}

public class QueryServiceRecordsHandler : IRequestHandler<QueryServiceRecords, ApiResponse<List<Servicing>>>
{
    private readonly IMachineRepository _repository;

    public QueryServiceRecordsHandler(IMachineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<List<Servicing>>> Handle(QueryServiceRecords request, CancellationToken cancellationToken)
    {
        var records = await _repository.GetServicingRecordsAsync(request.MachineId);
        return new ApiResponse<List<Servicing>>(records);
    }
}