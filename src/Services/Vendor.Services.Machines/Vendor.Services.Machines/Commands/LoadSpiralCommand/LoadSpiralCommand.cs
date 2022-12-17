using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Data.Persistence;
using Vendor.Services.Machines.Data.Repositories.Interfaces;
using Vendor.Services.Machines.Views;

namespace Vendor.Services.Machines.Commands.LoadSpiralCommand;

public class LoadSpiralCommand : IRequest<ApiResponse<SpiralView>>
{
    public string Title { get; set; }
    public string Spiral { get; set; }
    public int ProductId { get; set; }
    public int Loads { get; set; }
    public Double Price { get; set; }
}

public class LoadSpiralCommandHandler : IRequestHandler<LoadSpiralCommand, ApiResponse<SpiralView>>
{
    private readonly MachineDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public LoadSpiralCommandHandler(MachineDbContext context, IProductRepository productRepository, IMapper mapper)
    {
        _context = context;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<SpiralView>> Handle(LoadSpiralCommand request, CancellationToken cancellationToken)
    {
        var vending = await _context.Vendings.Include(v => v.Spirals).FirstAsync(v => v.Title == request.Title, cancellationToken);
        var spiral = vending.Spirals.First(s => s.Name == request.Spiral);

        //Set loads per current spiral in the vending tray
        spiral.Loads = request.Loads;
        //Set specified product as being sold in the current spiral
        spiral.ProductId = request.ProductId;
        //Set the price for the product in the spiral
        spiral.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);

        return new ApiResponse<SpiralView>(_mapper.Map<SpiralView>(spiral), "Spiral loaded");
    }
}