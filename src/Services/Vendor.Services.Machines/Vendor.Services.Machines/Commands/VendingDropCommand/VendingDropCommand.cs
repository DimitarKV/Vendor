using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Commands.VendingDropCommand;

public class VendingDropCommand : IRequest<ApiResponse>
{
    public string Title { get; set; }
    public string Spiral { get; set; }
}

public class VendingDropCommandHandler : IRequestHandler<VendingDropCommand, ApiResponse>
{
    private readonly MachineDbContext _context;

    public VendingDropCommandHandler(MachineDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse> Handle(VendingDropCommand request, CancellationToken cancellationToken)
    {
        var vending = await _context.Vendings.FirstAsync(v => v.Title == request.Title, cancellationToken);
        vending.Spirals.First(s => s.Name == request.Spiral).Loads--;
        await _context.SaveChangesAsync(cancellationToken);
        
        return new ApiResponse();
    }
}