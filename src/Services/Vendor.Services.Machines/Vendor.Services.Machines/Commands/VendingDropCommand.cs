using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Commands;

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

public class VendingDropCommandValidator : AbstractValidator<VendingDropCommand>
{
    public VendingDropCommandValidator(MachineDbContext context)
    {
        RuleFor(c => c)
            .Cascade(CascadeMode.Stop)

            // Check whether the given machine title exists in the database
            .MustAsync(async (c, _) =>
                await context.Vendings.FirstOrDefaultAsync(v => v.Title == c.Title) is not null)
            .WithMessage("No such vending machine with the given title exists")
            .WithErrorCode("409")

            // Check whether the given spiral name exists in the current vending machine
            .MustAsync(async (c, _) =>
                (await context.Vendings.Include(v => v.Spirals)
                    .FirstAsync(v => v.Title == c.Title))
                .Spirals.Exists(s => s.Name == c.Spiral)
            )
            .WithMessage("No such spiral in the database")
            .WithErrorCode("409")

            // Check whether the spiral has sufficient products
            .MustAsync(async (c, _) =>
                (await context.Vendings.Include(v => v.Spirals).FirstAsync(v => v.Title == c.Title))
                .Spirals
                .First(s => s.Name == c.Spiral)
                .Loads > 0)
            .WithMessage("Spiral empty")
            .WithErrorCode("409");
    }
}