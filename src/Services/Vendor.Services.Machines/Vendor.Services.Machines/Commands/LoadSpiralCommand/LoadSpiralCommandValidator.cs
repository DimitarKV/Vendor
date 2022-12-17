using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Commands.LoadSpiralCommand;

public class LoadSpiralCommandValidator : AbstractValidator<LoadSpiralCommand>
{
    public LoadSpiralCommandValidator(MachineDbContext context)
    {
        RuleFor(v => v.Title)
            .MustAsync(async (title, _) =>
                await context.Vendings.FirstOrDefaultAsync(v => v.Title == title) is not null)
            .WithMessage("No such vending machine in the database!")
            .WithErrorCode("409");
    }
}