using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Queries;

public class QuerySpiral : IRequest<ApiResponse<SpiralView>>
{
    public int SpiralId { get; set; }
}

public class QuerySpiralHandler : IRequestHandler<QuerySpiral, ApiResponse<SpiralView>>
{
    private readonly IMapper _mapper;
    private readonly IMachineRepository _repository;

    public QuerySpiralHandler(IMapper mapper, IMachineRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResponse<SpiralView>> Handle(QuerySpiral request, CancellationToken cancellationToken)
    {
        var spiral = await _repository.GetSpiralByIdAsync(request.SpiralId);
        return new ApiResponse<SpiralView>(_mapper.Map<SpiralView>(spiral), "Here u go buddy!");
    }
}

public class QuerySpiralValidator : AbstractValidator<QuerySpiral>
{
    public QuerySpiralValidator(IMachineRepository repository)
    {
        RuleFor(q => q)
            .MustAsync(async (q, _) =>
                (await repository.GetSpiralByIdAsync(q.SpiralId) is not null))
            .WithMessage("No such spiral in the machine!")
            .WithErrorCode("409");
    }
}