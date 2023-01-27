using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vendor.Domain.Types;
using Vendor.Services.User.Identity;

namespace Vendor.Services.User.Commands.CreateTokenCommand;

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
    private readonly IIdentityService _identityService;

    //TODO extract as global
    private const int ExpirationInMinutes = 30;
    
    public CreateTokenCommandHandler(IConfiguration configuration, IIdentityService identityService)
    {
        _configuration = configuration;
        _identityService = identityService;
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
        
        var user = await _identityService.FindByNameAsync(request.UserName);
        
        var claims = await _identityService.GetClaimsAsync(user);
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