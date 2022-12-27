using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Queries;

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