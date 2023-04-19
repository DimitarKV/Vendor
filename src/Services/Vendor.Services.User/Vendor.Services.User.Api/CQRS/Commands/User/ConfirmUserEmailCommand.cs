using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vendor.Domain.Types;

namespace Vendor.Services.User.Api.CQRS.Commands.User;

public class ConfirmUserEmailCommand : IRequest<ApiResponse>
{
    public string Username { get; set; }
    public string Token { get; set; }

    public ConfirmUserEmailCommand()
    {
        
    }

    public ConfirmUserEmailCommand(string username, string token)
    {
        Username = username;
        Token = token;
    }
}

public class ConfirmUserEmailCommandHandler : IRequestHandler<ConfirmUserEmailCommand, ApiResponse>
{
    private readonly UserManager<VendorUser> _userManager;

    public ConfirmUserEmailCommandHandler(UserManager<VendorUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponse> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        var result = await _userManager.ConfirmEmailAsync(user!, request.Token);

        if (result.Succeeded)
            return new ApiResponse();
        
        return new ApiResponse("Error", result.Errors.Select(e => e.Description));
    }
}

public class ConfirmUserEmailCommandValidator : AbstractValidator<ConfirmUserEmailCommand>
{
    public ConfirmUserEmailCommandValidator(UserManager<VendorUser> userManager)
    {
        RuleFor(c => c)
            .MustAsync(async (c, _) =>
                await userManager.FindByNameAsync(c.Username) is not null)
            .WithMessage("No such user in the database")
            .WithErrorCode("401");
    }
}