using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Vendor.Domain.Types;

namespace Vendor.Services.User.Api.CQRS.Commands.Token;

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

    public CreateTokenCommandHandler(IConfiguration configuration, UserManager<VendorUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    
    public async Task<ApiResponse<string>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, 
            SecurityAlgorithms.HmacSha256);
        
        var user = await _userManager.FindByNameAsync(request.UserName);
        
        var claims = await _userManager.GetClaimsAsync(user!);
        claims.Add(new Claim(ClaimTypes.Name, request.UserName));
        var role = claims.First(c => c.Type == ClaimTypes.Role).Value;
        var expirationTime = Claims.RoleClaims[role].SessionExpiry;
        
        var token = new JwtSecurityToken(issuer: issuer,
            audience: audience,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(expirationTime),
            claims: claims);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);

        return new ApiResponse<string>(stringToken, "Successfully created a token");
    }
}

public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
    public CreateTokenCommandValidator(UserManager<VendorUser> userManager)
    {
        RuleFor(x => new {x.UserName, x.Password})
            .Cascade(CascadeMode.Stop)
            
            .MustAsync(async (pair, _) => await userManager.FindByNameAsync(pair.UserName) is not null)
            .WithErrorCode("401")
            .WithMessage("Wrong credentials")
            
            .MustAsync(async (pair, _) =>
                await userManager.CheckPasswordAsync((await userManager.FindByNameAsync(pair.UserName))!, pair.Password))
            
            .WithErrorCode("401")
            .WithMessage("Wrong credentials");
    }
}