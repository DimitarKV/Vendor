using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Services.User.Authorization;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.Commands.User;

public class CreateUserCommand : IRequest<ApiResponse<VendorUser?>>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public CreateUserCommand()
    {
    }

    public CreateUserCommand(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<VendorUser?>>
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
    public async Task<ApiResponse<VendorUser?>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<VendorUser>(request);

        user.CreatedOn = DateTime.Now;
        user.UpdatedOn = DateTime.Now;

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new ApiResponse<VendorUser?>(null, "An error occurred while creating a user",
                result.Errors.Select(x => x.Description));

        Claim role = Claims.RoleClaims["User"];
        if (_userManager.Users.Count() == 1)
        {
            role = Claims.RoleClaims["User"];
            role = Claims.RoleClaims["Maintainer"];
            role = Claims.RoleClaims["Admin"];
        }
        await _userManager.AddClaimAsync(user, role);

        return new ApiResponse<VendorUser?>(user, "Successfully created a user");
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(UserManager<VendorUser> userManager)
    {
        RuleFor(command => command.UserName)
            .MustAsync(async (userName, _) =>
                await userManager.FindByNameAsync(userName) is null)
            .WithErrorCode("401")
            .WithMessage("User already exists");
    }
}