using MediatR;
using Vendor.Domain.Types;
using Vendor.Services.User.Identity;

namespace Vendor.Services.User.Commands.User.GenerateConfirmationTokenCommand;

public class GenerateConfirmationTokenCommand : IRequest<ApiResponse<string>>
{
    public string Username { get; set; }
}

public class
    GenerateConfirmationTokenCommandHandler : IRequestHandler<GenerateConfirmationTokenCommand, ApiResponse<string>>
{
    private readonly IIdentityService _identityService;

    public GenerateConfirmationTokenCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    //TODO: Validation for missing user
    public async Task<ApiResponse<string>> Handle(GenerateConfirmationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByNameAsync(request.Username);
        
        var token = await _identityService.GenerateEmailConfirmationTokenAsync(user!);

        return new ApiResponse<string>(token);
    }
}