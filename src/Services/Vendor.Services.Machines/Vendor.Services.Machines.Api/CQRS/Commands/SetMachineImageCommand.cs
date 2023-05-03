using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Commands;

public class SetMachineImageCommand : IRequest<ApiResponse<VendingView>>
{
    public int MachineId { get; set; }
    public string ImageUrl { get; set; }
}

public class SetMachineImageCommandHandler : IRequestHandler<SetMachineImageCommand, ApiResponse<VendingView>>
{
    private readonly IMachineRepository _repository;
    private readonly IMapper _mapper;

    public SetMachineImageCommandHandler(IMachineRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<VendingView>> Handle(SetMachineImageCommand request, CancellationToken cancellationToken)
    {
        var machine = await _repository.SetVendingImageUrl(request.MachineId, request.ImageUrl);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        if (machine is null)
            return new ApiResponse<VendingView>(null, "Error", new []{"A machine with the given id couldn't be retrieved!"});

        return new ApiResponse<VendingView>(_mapper.Map<VendingView>(machine));
    }
}

public class SetMachineImageCommandValidator : AbstractValidator<SetMachineImageCommand>
{
    public SetMachineImageCommandValidator(IMachineRepository repository)
    {
        RuleFor(m => m)
            .MustAsync(async (m, _) => (await repository.GetVendingByIdAsync(m.MachineId)) is not null)
            .WithMessage("No such machine exists!")
            .WithErrorCode("401");
    }
}