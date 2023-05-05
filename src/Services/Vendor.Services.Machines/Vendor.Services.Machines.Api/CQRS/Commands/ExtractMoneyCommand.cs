using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Commands;

public class ExtractMoneyCommand : IRequest<ApiResponse<VendingView>>
{
    public int VendingId { get; set; }
    public Decimal Amount { get; set; }
}

public class ExtractMoneyCommandHandler : IRequestHandler<ExtractMoneyCommand, ApiResponse<VendingView>>
{
    private readonly IMachineRepository _repository;
    private readonly IMapper _mapper;

    public ExtractMoneyCommandHandler(IMachineRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<VendingView>> Handle(ExtractMoneyCommand request, CancellationToken cancellationToken)
    {
        var vending = await _repository.GetVendingByIdAsync(request.VendingId);
        vending!.SubtractMoney(request.Amount);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiResponse<VendingView>(_mapper.Map<VendingView>(vending));
    }
}

public class ExtractMoneyCommandValidator : AbstractValidator<ExtractMoneyCommand>
{
    public ExtractMoneyCommandValidator(IMachineRepository repository)
    {
        RuleFor(c => c)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (c, _) => (await repository.GetVendingByIdAsync(c.VendingId)) is not null)
            .WithMessage("No such machine exists!")
            .WithErrorCode("400")
            .MustAsync(async (c, _) =>
                (await repository.GetVendingByIdAsync(c.VendingId))!.Money >= c.Amount)
            .WithMessage("Not enough balance in the machine!")
            .WithErrorCode("400");
    }
}