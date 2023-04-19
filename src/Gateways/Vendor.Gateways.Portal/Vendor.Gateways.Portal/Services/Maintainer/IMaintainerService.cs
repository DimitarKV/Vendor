using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.Wrappers.ResponseTypes;

namespace Vendor.Gateways.Portal.Services.Maintainer;

public interface IMaintainerService
{
    Task<ClientResponse<ApiResponse<List<VendingView>>>> FetchEmptyMachines();

    Task<ClientResponse<ApiResponse<HandleView>>> HandleMachine(int id); 
}