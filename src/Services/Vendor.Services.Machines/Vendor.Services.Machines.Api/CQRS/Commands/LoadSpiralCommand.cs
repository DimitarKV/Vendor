using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Commands;

public class LoadSpiralCommand : IRequest<ApiResponse<SpiralView>>
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Loads { get; set; }
    public Decimal Price { get; set; }
}

public class LoadSpiralCommandHandler : IRequestHandler<LoadSpiralCommand, ApiResponse<SpiralView>>
{
    private readonly IMapper _mapper;
    private readonly IMachineRepository _repository;

    public LoadSpiralCommandHandler(IMapper mapper, IMachineRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResponse<SpiralView>> Handle(LoadSpiralCommand request, CancellationToken cancellationToken)
    {
        await _repository.LoadSpiralAsync(request.Id, request.ProductId, request.Loads, request.Price);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        var spiral = await _repository.GetSpiralByIdAsync(request.Id);
        return new ApiResponse<SpiralView>(_mapper.Map<SpiralView>(spiral), "Spiral loaded");
    }
}

public class LoadSpiralCommandValidator : AbstractValidator<LoadSpiralCommand>
{
    //TODO: add more validations for price and loads
    public LoadSpiralCommandValidator(IMachineRepository repository)
    {
        RuleFor(v => v)

            .MustAsync(async (ls, _) => 
                (await repository.GetSpiralByIdAsync(ls.Id) is not  null))
            .WithMessage("No such spiral in the specified machine")
            .WithErrorCode("409");
    }
}