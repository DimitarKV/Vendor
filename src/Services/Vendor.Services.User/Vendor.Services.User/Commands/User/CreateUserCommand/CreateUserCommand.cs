using System.Security.Policy;
using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Vendor.Domain.Types;
using Vendor.Services.User.Authorization;
using Vendor.Services.User.Data.Entities;
using Vendor.Services.User.Identity;

namespace Vendor.Services.User.Commands.User.CreateUserCommand;

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
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;

    public CreateUserCommandHandler(IMapper mapper, IIdentityService identityService, IConfiguration configuration, IEmailSender emailSender)
    {
        _mapper = mapper;
        _identityService = identityService;
        _configuration = configuration;
        _emailSender = emailSender;
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

        var result = await _identityService.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new ApiResponse<VendorUser?>(null, "An error occurred while creating a user",
                result.Errors.Select(x => x.Description));

        await _identityService.AddClaimAsync(user, Claims.User);
        
        return new ApiResponse<VendorUser?>(user, "Successfully created a user");
    }
}