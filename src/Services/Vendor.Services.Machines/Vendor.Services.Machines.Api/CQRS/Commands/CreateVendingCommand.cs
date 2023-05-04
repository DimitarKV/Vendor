using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Commands;

public class CreateVendingCommand : IRequest<ApiResponse<VendingView>>
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public int Spirals { get; set; }
    public int SpiralsPerRow { get; set; }

    public CreateVendingCommand()
    {
        
    }
    
    public CreateVendingCommand(string title, double latitude, double longitude)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
    }

    public CreateVendingCommand(string title, double latitude, double longitude, int spirals)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
        Spirals = spirals;
    }
}

public class CreateVendingCommandHandler : IRequestHandler<CreateVendingCommand, ApiResponse<VendingView>>
{
    private readonly IMapper _mapper;
    private readonly IMachineRepository _repository;

    public CreateVendingCommandHandler(IMapper mapper, IMachineRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResponse<VendingView>> Handle(CreateVendingCommand request,
        CancellationToken cancellationToken)
    {
        var vending = _repository.CreateVending(request.Title, request.Spirals, request.Latitude, request.Longitude, request.SpiralsPerRow);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var vendingView = _mapper.Map<VendingView>(vending);

        return new ApiResponse<VendingView>(vendingView);
    }
}

public class CreateVendingCommandValidator : AbstractValidator<CreateVendingCommand>
{
    public CreateVendingCommandValidator(IMachineRepository repository)
    {
        RuleFor(v => v.Title)
            .Must(title => title.Length >= 4)
            .WithErrorCode("409")
            .WithMessage("Title length must be at least 4 characters!");
    }
}