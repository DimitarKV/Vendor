using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

public interface IMachineRepository : IRepository
{
    public Vending CreateVending(string title, int spiralCount, Double latitude, Double longitude, int spiralsPerRow);
    public Task<bool> LoadSpiralAsync(int spiralId, int productId, int loads, Decimal price);
    public Task DropProductAsync(int spiralId);
    public Task<List<Vending>> GetEmptyVendingsAsync();
    public Task<List<Vending>> GetNonEmptyVendingsAsync();
    public Task<Vending?> GetVendingByIdAsync(int id);
    public Task<Vending?> GetVendingAndSpiralsByIdAsync(int id);
    public Task<Spiral?> GetSpiralByIdAsync(int id);
    public Task<Spiral?> GetSpiralAndVendingByIdAsync(int id);
    public Task<Vending?> SetVendingImageUrl(int machineId, string url);

    public Servicing CreateServicingRecord(int machineId, string maintainerUserName, DateTime time, string fillings);
    public Task<List<Servicing>> GetServicingRecordsAsync(int machineId);

}