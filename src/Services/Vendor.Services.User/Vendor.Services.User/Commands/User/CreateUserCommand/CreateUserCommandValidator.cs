using FluentValidation;
using Vendor.Services.User.Identity;

namespace Vendor.Services.User.Commands.User.CreateUserCommand;

public class CreateUserCommandValidator : AbstractValidator<Vendor.Services.User.Commands.User.CreateUserCommand.CreateUserCommand>
{
    public CreateUserCommandValidator(IIdentityService identityService)
    {
        RuleFor(command => command.UserName)
            .MustAsync(async (userName, _) =>
                await identityService.FindByNameAsync(userName) is null)
            .WithErrorCode("400")
            .WithMessage("User already exists");
    }
}