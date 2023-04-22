using AutoMapper;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.AggregateModel.HandleAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Commands;

public class HandleVendingCommand : IRequest<ApiResponse<HandleView>>
{
    public string MaintainerId { get; set; }
    public int VendingId { get; set; }
    public TimeSpan Duration { get; set; }
}

public class HandleVendingCommandHandler : IRequestHandler<HandleVendingCommand, ApiResponse<HandleView>>
{
    private readonly IHandleRepository _repository;
    private readonly IMapper _mapper;

    public HandleVendingCommandHandler(IHandleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<HandleView>> Handle(HandleVendingCommand request, CancellationToken cancellationToken)
    {
        var handle = _repository.HandleMachine(request.VendingId, request.MaintainerId, request.Duration);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return new ApiResponse<HandleView>(_mapper.Map<HandleView>(handle));
    }
}