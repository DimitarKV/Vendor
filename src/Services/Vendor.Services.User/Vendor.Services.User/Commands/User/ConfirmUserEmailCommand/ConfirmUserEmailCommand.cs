using MediatR;
using Vendor.Domain.Types;
using Vendor.Services.User.Identity;

namespace Vendor.Services.User.Commands.User.ConfirmUserEmailCommand;

public class ConfirmUserEmailCommand : IRequest<ApiResponse>
{
    public string Username { get; set; }
    public string Token { get; set; }

    public ConfirmUserEmailCommand()
    {
        
    }

    public ConfirmUserEmailCommand(string username, string token)
    {
        Username = username;
        Token = token;
    }
}

public class ConfirmUserEmailCommandHandler : IRequestHandler<ConfirmUserEmailCommand, ApiResponse>
{
    private readonly IIdentityService _identityService;

    public ConfirmUserEmailCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    //TODO: Missing user validation
    public async Task<ApiResponse> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByNameAsync(request.Username);
        var result = await _identityService.ConfirmEmailAsync(user!, request.Token);

        if (result.Succeeded)
            return new ApiResponse();
        
        return new ApiResponse("Error", result.Errors.Select(e => e.Description));
    }
}