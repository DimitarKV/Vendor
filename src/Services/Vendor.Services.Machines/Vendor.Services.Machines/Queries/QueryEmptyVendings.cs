using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Queries;

public class QueryEmptyVendings : IRequest<ApiResponse<List<VendingView>>>
{
}

public class QueryEmptyVendingsHandler : IRequestHandler<QueryEmptyVendings, ApiResponse<List<VendingView>>>
{
    private readonly MachineDbContext _context;
    private readonly IMapper _mapper;

    public QueryEmptyVendingsHandler(MachineDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<VendingView>>> Handle(QueryEmptyVendings request, CancellationToken cancellationToken)
    {
        var emptyVendingsViews = await _context.Vendings.Include(v => v.Spirals)
            .Where(v => v.Spirals.Any(s => s.Loads <= 0))
            .Select(v => _mapper.Map<VendingView>(v))
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new ApiResponse<List<VendingView>>(emptyVendingsViews, "Successfully queried all empty vendings!");
    }
}