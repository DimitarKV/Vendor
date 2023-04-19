using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vendor.Domain.Types;

namespace Vendor.Services.User.Api.CQRS.Commands.User;

public class GenerateConfirmationTokenCommand : IRequest<ApiResponse<string>>
{
    public string Username { get; set; }
}

public class
    GenerateConfirmationTokenCommandHandler : IRequestHandler<GenerateConfirmationTokenCommand, ApiResponse<string>>
{
    private readonly UserManager<VendorUser> _userManager;

    public GenerateConfirmationTokenCommandHandler(UserManager<VendorUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponse<string>> Handle(GenerateConfirmationTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

        return new ApiResponse<string>(token);
    }
}

public class GenerateConfirmationTokenCommandValidator : AbstractValidator<GenerateConfirmationTokenCommand>
{
    public GenerateConfirmationTokenCommandValidator(UserManager<VendorUser> userManager)
    {
        RuleFor(c => c)
            .MustAsync(async (c, _) =>
                await userManager.FindByNameAsync(c.Username) is not null)
            .WithMessage("No such user in the database")
            .WithErrorCode("401");
    }
}