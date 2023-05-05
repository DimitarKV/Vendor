using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Gateways.Portal.DTO;

namespace Vendor.Gateways.Portal.Services.Maintainer;

public interface IMaintainerService
{
    Task<ApiResponse<List<VendingView>>> FetchEmptyMachines();
    Task<ApiResponse<List<VendingView>>> FetchNonEmptyMachines();
    
    Task<ApiResponse<VendingView>> CreateMachineAsync(CreateVendingRequestDto requestDto);
    Task<ApiResponse<VendingView>> SetMachineImageAsync(SetMachineImageDto requestDto);

    Task<ApiResponse<HandleView>> HandleMachineAsync(int id);
    Task<ApiResponse<VendingView>> FetchMachineById(int machineId);
    Task<ApiResponse<List<int>>> QueryMissingProductsAsync(int machineId);
    Task<ApiResponse<SpiralView>> LoadSpiralAsync(SpiralView spiral);
    Task<ApiResponse<List<SpiralView>>> LoadSpiralsAsync(List<SpiralView> spiral);

    Task<ApiResponse<List<ServiceRecordDto>>> FetchServiceRecords(int machineId);
    Task<ApiResponse<VendingView>> ExtractMoney(ExtractMoneyDto dto);
}