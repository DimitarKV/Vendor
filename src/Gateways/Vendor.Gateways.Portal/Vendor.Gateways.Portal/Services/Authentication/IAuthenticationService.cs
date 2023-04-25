using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;

namespace Vendor.Gateways.Portal.Services.Authentication;

public interface IAuthenticationService
{
    Task<ApiResponse<UserView>> RegisterAsync(RegisterUserFormData createUserForm);
    Task<ApiResponse<string>> LoginAsync(LoginUserFormData loginForm);
    Task<ApiResponse> ConfirmEmailAsync(ConfirmEmailDto dto);
}