using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;
using Vendor.Gateways.Portal.Wrappers.ResponseTypes;

namespace Vendor.Gateways.Portal.Services.Authentication;

public interface IAuthenticationService
{
    Task<ClientResponse<ApiResponse<UserView>>> RegisterAsync(RegisterUserFormData createUserForm);
    Task<ClientResponse<ApiResponse<string>>> LoginAsync(LoginUserFormData loginForm);
    Task<ClientResponse<ApiResponse>> ConfirmEmailAsync(ConfirmEmailDto dto);
}