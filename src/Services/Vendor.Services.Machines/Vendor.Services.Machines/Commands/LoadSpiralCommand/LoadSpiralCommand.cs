using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Commands.LoadSpiralCommand;

public class LoadSpiralCommand : IRequest<ApiResponse>
{
    public string Title { get; set; }
    public string Spiral { get; set; }
    public int ProductId { get; set; }
    public int Loads { get; set; }
    public int Price { get; set; }
}

//TODO: create validations for missing vending title in the database + missing product id
public class LoadSpiralCommandHandler : IRequestHandler<LoadSpiralCommand, ApiResponse>
{
    private readonly MachineDbContext _context;

    public LoadSpiralCommandHandler(MachineDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse> Handle(LoadSpiralCommand request, CancellationToken cancellationToken)
    {
        var vending = await _context.Vendings.FirstAsync(v => v.Title == request.Title, cancellationToken);
        var spiral = vending.Spirals.First(s => s.Name == request.Spiral);
        
        //Set loads per current spiral in the vending tray
        spiral.Loads = request.Loads;
        //Set specified product as being sold in the current spiral
        spiral.ProductId = request.ProductId;
        //Set the price for the product in the spiral
        spiral.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);
        
        return new ApiResponse(
            $@"Spiral {request.Spiral} loaded successfully with {request.Loads} at a price of {request.Price}.");
    }
}