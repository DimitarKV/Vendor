using AutoMapper;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.Data.Persistence;
using Vendor.Services.Machines.Data.Repositories.Interfaces;
using Vendor.Services.Machines.Views;

namespace Vendor.Services.Machines.Commands.CreateVendorCommand;

public class CreateVendingCommand : IRequest<ApiResponse<VendingView>>
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }

    public CreateVendingCommand()
    {
        Title = "";
    }

    public CreateVendingCommand(string title, double latitude, double longitude)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class CreateVendingCommandHandler : IRequestHandler<CreateVendingCommand, ApiResponse<VendingView>>
{
    private readonly IMapper _mapper;
    private readonly IVendingRepository _repository;
    private readonly MachineDbContext _context;

    public CreateVendingCommandHandler(IMapper mapper, IVendingRepository repository, MachineDbContext context)
    {
        _mapper = mapper;
        _repository = repository;
        _context = context;
    }

    public async Task<ApiResponse<VendingView>> Handle(CreateVendingCommand request, CancellationToken cancellationToken)
    {
        var vending = _mapper.Map<Vending>(request);
        vending.Money = 0;
        
        var result = _context.Vendings.Add(vending).Entity;
        await _context.SaveChangesAsync(cancellationToken);

        var vendingView = _mapper.Map<VendingView>(result);
        
        return new ApiResponse<VendingView>(vendingView);
    }
}