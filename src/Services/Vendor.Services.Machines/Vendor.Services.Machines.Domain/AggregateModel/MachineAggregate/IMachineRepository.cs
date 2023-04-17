using Vendor.Services.Machines.SeedWork;

namespace Vendor.Services.Machines.AggregateModel.MachineAggregate;

public interface IMachineRepository : IRepository
{
    public Vending CreateVending(string title, int spiralCount, Double latitude, Double longitude);
    public Task LoadSpiralAsync(int spiralId, int productId, int loads, Double price);
    public Task DropProductAsync(int spiralId);
    public Task<List<Vending>> GetEmptyVendingsAsync();
    public Task<Vending?> GetVendingByIdAsync(int id);
    public Task<Spiral?> GetSpiralByIdAsync(int id);
    public Task<Spiral?> GetSpiralAndVendingByIdAsync(int id);
}