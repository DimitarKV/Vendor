using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Models;

namespace Vendor.Gateways.Portal.Services.Authentication;

public interface IAuthenticationService
{
    Task<ApiResponse<UserView>> RegisterAsync(RegisterUserModel createUserForm);
    Task<ApiResponse<string>> LoginAsync(LoginUserModel loginForm);
    Task<ApiResponse> ConfirmEmailAsync(ConfirmEmailRequestDto requestDto);
}