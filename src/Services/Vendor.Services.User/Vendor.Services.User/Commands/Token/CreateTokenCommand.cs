using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vendor.Domain.Types;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.Commands.Token;

public class CreateTokenCommand : IRequest<ApiResponse<string>>
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public CreateTokenCommand()
    {
        
    }

    public CreateTokenCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}

public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, ApiResponse<string>>
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<VendorUser> _userManager;

    //TODO extract as global
    private const int ExpirationInMinutes = 30;
    
    public CreateTokenCommandHandler(IConfiguration configuration, UserManager<VendorUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Creates token after login operation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse<string>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, 
            SecurityAlgorithms.HmacSha256);
        
        var user = await _userManager.FindByNameAsync(request.UserName);
        
        var claims = await _userManager.GetClaimsAsync(user);
        claims.Add(new Claim(ClaimTypes.Name, request.UserName));
        
        var token = new JwtSecurityToken(issuer: issuer,
            audience: audience,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(ExpirationInMinutes),
            claims: claims);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);

        return new ApiResponse<string>(stringToken, "Successfully created a token");
    }
}

public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
    public CreateTokenCommandValidator(UserManager<VendorUser> _userManager)
    {
        RuleFor(x => new {x.UserName, x.Password})
            .Cascade(CascadeMode.Stop)
            
            .MustAsync(async (pair, _) => await _userManager.FindByNameAsync(pair.UserName) is not null)
            
            .MustAsync(async (pair, _) =>
                await _userManager.CheckPasswordAsync((await _userManager.FindByNameAsync(pair.UserName))!, pair.Password))
            
            .WithErrorCode("401")
            .WithMessage("Wrong credentials");
    }
}