using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Commands.VendingDropCommand;

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