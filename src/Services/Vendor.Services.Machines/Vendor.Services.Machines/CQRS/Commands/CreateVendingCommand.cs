using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.Data.Persistence;

namespace Vendor.Services.Machines.Commands;

public class CreateVendingCommand : IRequest<ApiResponse<VendingView>>
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public int Spirals { get; set; }

    public CreateVendingCommand()
    {
        Title = "";
    }

    public CreateVendingCommand(string title, double latitude, double longitude)
    {
        Title = title;
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class CreateVendingCommandHandler : IRequestHandler<CreateVendingCommand, ApiResponse<VendingView>>
{
    private readonly IMapper _mapper;
    private readonly MachineDbContext _context;

    public CreateVendingCommandHandler(IMapper mapper, MachineDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ApiResponse<VendingView>> Handle(CreateVendingCommand request,
        CancellationToken cancellationToken)
    {
        var vending = _mapper.Map<Vending>(request);
        vending.Money = 0;

        for (int j = 0; j < request.Spirals; j++)
        {
            var spiral = new Spiral(){Name = j.ToString()};
            vending.Spirals.Add(spiral);
        }

        var result = _context.Vendings.Add(vending).Entity;
        await _context.SaveChangesAsync(cancellationToken);

        var vendingView = _mapper.Map<VendingView>(result);

        return new ApiResponse<VendingView>(vendingView);
    }
}

public class CreateVendingCommandValidator : AbstractValidator<CreateVendingCommand>
{
    public CreateVendingCommandValidator(MachineDbContext context)
    {
        RuleFor(v => v.Title)
            .MustAsync(async (title, _) =>
                await context.Vendings.Where(v => v.Title == title).FirstOrDefaultAsync() is null)
            .WithErrorCode("409")
            .WithMessage("Title is not available!");

        RuleFor(v => v.Title)
            .Must(title => title.Length >= 4)
            .WithErrorCode("409")
            .WithMessage("Title length must be at least 4 characters!");
    }
}