using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.User.Authorization;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.Commands.User;

public class CreateUserCommand : IRequest<ApiResponse<UserView?>>
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public CreateUserCommand()
    {
    }

    public CreateUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<UserView?>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<VendorUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;

    public CreateUserCommandHandler(IMapper mapper, IConfiguration configuration, IEmailSender emailSender,
        UserManager<VendorUser> userManager)
    {
        _mapper = mapper;
        _configuration = configuration;
        _emailSender = emailSender;
        _userManager = userManager;
    }

    /// <summary>
    /// Creates user with the given credentials 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<UserView?>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<VendorUser>(request);

        if ((await _userManager.FindByNameAsync(request.Username)) is not null)
        {
            await _userManager.DeleteAsync((await _userManager.FindByNameAsync(request.Username))!);
        }

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new ApiResponse<UserView?>(null, "An error occurred while creating a user",
                result.Errors.Select(x => x.Description));

        Claim role = Claims.RoleClaims["User"].Claim;
        if (_userManager.Users.Count() == 1)
        {
            role = Claims.RoleClaims["Admin"].Claim;
        }

        await _userManager.AddClaimAsync(user, role);

        return new ApiResponse<UserView?>(_mapper.Map<UserView>(user), "Successfully created a user");
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(UserManager<VendorUser> userManager)
    {
        RuleFor(command => command.Username)
            .MustAsync(async (username, _) =>
                await userManager.FindByNameAsync(username) is null
                ||
                (
                    (await userManager.FindByNameAsync(username))!.EmailConfirmed == false
                    &&
                    DateTime.Now.Subtract((await userManager.FindByNameAsync(username))!.CreatedOn).Days > 7
                )
            )
            .WithErrorCode("401")
            .WithMessage("User already exists");
    }
}