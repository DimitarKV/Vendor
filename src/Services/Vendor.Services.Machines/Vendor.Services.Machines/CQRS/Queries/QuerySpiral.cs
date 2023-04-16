using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Data.Persistence;
using Vendor.Services.Machines.Views;

namespace Vendor.Services.Machines.Queries;

public class QuerySpiral : IRequest<ApiResponse<SpiralView>>
{
    public string Title { get; set; }
    public string Name { get; set; }
}

public class QuerySpiralHandler : IRequestHandler<QuerySpiral, ApiResponse<SpiralView>>
{
    private readonly MachineDbContext _context;
    private readonly IMapper _mapper;

    public QuerySpiralHandler(MachineDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<SpiralView>> Handle(QuerySpiral request, CancellationToken cancellationToken)
    {
        var spiral = (await _context.Vendings.Include(v => v.Spirals)
                .FirstAsync(v => v.Title == request.Title, cancellationToken: cancellationToken))
            .Spirals.First(s => s.Name == request.Name);
        return new ApiResponse<SpiralView>(_mapper.Map<SpiralView>(spiral), "Here u go buddy!");
    }
}

public class QuerySpiralValidator : AbstractValidator<QuerySpiral>
{
    public QuerySpiralValidator(MachineDbContext context)
    {
        RuleFor(q => q)
            .Cascade(CascadeMode.Stop)
            
            .MustAsync(async (q, _) =>
                await context.Vendings.FirstOrDefaultAsync(v => v.Title == q.Title) is not null)
            .WithMessage("No such vending machine in the database!")
            .WithErrorCode("409")
            
            .MustAsync(async (q, _) =>
                (await context.Vendings.Include(v => v.Spirals)
                    .FirstOrDefaultAsync(v => v.Title == q.Title))!.Spirals
                .FirstOrDefault(s => s.Name == q.Name) is not null)
            .WithMessage("No such spiral in the machine!")
            .WithErrorCode("409");
    }
}