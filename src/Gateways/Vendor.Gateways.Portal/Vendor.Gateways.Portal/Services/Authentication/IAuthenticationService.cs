using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;

namespace Vendor.Gateways.Portal.Services.Authentication;

public interface IAuthenticationService
{
    Task<ApiResponse<UserView>> Register(RegisterUserFormData createUserForm);
}