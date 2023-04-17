using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Vendor.Domain.Types;
using Vendor.Services.User.Authorization;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.CQRS.Commands.User;

public class AddRoleToUserCommand : IRequest<ApiResponse>
{
    public string Username { get; set; }
    public string Role { get; set; }
}

public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, ApiResponse>
{
    private readonly UserManager<VendorUser> _userManager;

    public AddRoleToUserCommandHandler(UserManager<VendorUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponse> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        foreach (var claim in await _userManager.GetClaimsAsync(user!))
        {
            if (claim.Type == ClaimTypes.Role)
                await _userManager.RemoveClaimAsync(user!, claim);
        }
        
        var identityResult =
            await _userManager.AddClaimAsync(user!, Claims.RoleClaims[request.Role].Claim);

        if (!identityResult.Succeeded)
            return new ApiResponse("Error", identityResult.Errors.Select(e => e.Description));

        return new ApiResponse($"Successfully added role {request.Role} to {request.Username}");
    }
}

public class AddRoleToUserCommandValidator : AbstractValidator<AddRoleToUserCommand>
{
    public AddRoleToUserCommandValidator(UserManager<VendorUser> userManager)
    {
        RuleFor(c => c)
            .Cascade(CascadeMode.Stop)
            
            .MustAsync(async (c, _) =>
                await userManager.FindByNameAsync(c.Username) is not null)
            .WithMessage("No such user in the database")
            .WithErrorCode("401")
            
            .Must(c =>
                Claims.RoleClaims.ContainsKey(c.Role))
            .WithMessage("There is no such role")
            .WithErrorCode("401")
            
            .MustAsync(async (c, _) =>
                (await userManager.GetClaimsAsync((await userManager.FindByNameAsync(c.Username))!))
                .Where(claim => claim.Type == ClaimTypes.Role).Where(r => r.Value == c.Role).IsNullOrEmpty())
            .WithMessage("The user already is in this role")
            .WithErrorCode("401");
    }
}