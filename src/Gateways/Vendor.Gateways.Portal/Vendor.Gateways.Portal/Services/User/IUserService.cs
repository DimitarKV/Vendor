using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;

namespace Vendor.Gateways.Portal.Services.User;

public interface IUserService
{
    public Task<ApiResponse<List<UserView>>> QueryUsersAsync(QueryUsersRequestDto requestDto);
    public Task<ApiResponse<List<string>>> QueryAvailableRolesAsync();
}