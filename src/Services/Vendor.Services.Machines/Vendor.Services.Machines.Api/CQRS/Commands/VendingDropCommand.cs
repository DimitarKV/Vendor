using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Commands;

public class VendingDropCommand : IRequest<ApiResponse>
{
    public int SpiralId { get; set; }
}

public class VendingDropCommandHandler : IRequestHandler<VendingDropCommand, ApiResponse>
{
    private readonly IMapper _mapper;
    private readonly IMachineRepository _repository;

    public VendingDropCommandHandler(IMapper mapper, IMachineRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResponse> Handle(VendingDropCommand request, CancellationToken cancellationToken)
    {
        await _repository.DropProductAsync(request.SpiralId);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        var spiral = await _repository.GetSpiralByIdAsync(request.SpiralId);
        var vending = spiral!.Vending;

        return new ApiResponse<VendingView>(_mapper.Map<VendingView>(vending), "Successfully dropped");
    }
}

public class VendingDropCommandValidator : AbstractValidator<VendingDropCommand>
{
    public VendingDropCommandValidator(IMachineRepository repository)
    {
        RuleFor(c => c)
            .Cascade(CascadeMode.Stop)

            // Check whether the given spiral name exists in the current vending machine
            .MustAsync(async (c, _) =>
                (await repository.GetSpiralByIdAsync(c.SpiralId) is not null))
            .WithMessage("No such spiral in the database")
            .WithErrorCode("409")

            // Check whether the spiral has sufficient products
            .MustAsync(async (c, _) =>
                !(await repository.GetSpiralByIdAsync(c.SpiralId))!.IsEmpty())
            .WithMessage("Spiral empty")
            .WithErrorCode("409");
    }
}