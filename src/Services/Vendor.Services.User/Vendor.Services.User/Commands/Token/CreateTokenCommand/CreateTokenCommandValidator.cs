using FluentValidation;
using Vendor.Services.User.Identity;

namespace Vendor.Services.User.Commands.Token.CreateTokenCommand;

public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
    public CreateTokenCommandValidator(IIdentityService identityService)
    {
        RuleFor(x => new {x.UserName, x.Password})
            .Cascade(CascadeMode.Stop)
            
            .MustAsync(async (pair, _) => await identityService.FindByNameAsync(pair.UserName) is not null)
            
            .MustAsync(async (pair, _) =>
                await identityService.CheckPasswordAsync((await identityService.FindByNameAsync(pair.UserName))!, pair.Password))
            
            .WithErrorCode("401")
            .WithMessage("Wrong credentials");
    }
}