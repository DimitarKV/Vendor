using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Data.Persistence;
using Vendor.Services.Machines.Views;

namespace Vendor.Services.Machines.Commands.VendingDropCommand;

public class VendingDropCommand : IRequest<ApiResponse>
{
    public string Title { get; set; }
    public string Spiral { get; set; }
}

public class VendingDropCommandHandler : IRequestHandler<VendingDropCommand, ApiResponse>
{
    private readonly MachineDbContext _context;
    private readonly IMapper _mapper;

    public VendingDropCommandHandler(MachineDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse> Handle(VendingDropCommand request, CancellationToken cancellationToken)
    {
        var vending = await _context.Vendings.Include(v => v.Spirals).FirstAsync(v => v.Title == request.Title, cancellationToken);
        var spiral = vending.Spirals.First(s => s.Name == request.Spiral);
        spiral.Loads--;

        await _context.SaveChangesAsync(cancellationToken);
        
        return new ApiResponse<VendingView>(_mapper.Map<VendingView>(vending), "Successfully dropped");
    }
}