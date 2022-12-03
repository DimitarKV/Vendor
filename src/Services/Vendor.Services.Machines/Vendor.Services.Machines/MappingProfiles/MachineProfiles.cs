using AutoMapper;
using Vendor.Services.Machines.Commands.CreateVendorCommand;
using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.Views;

namespace Vendor.Services.Machines.MappingProfiles;

public class MachineProfiles : Profile
{
    public MachineProfiles()
    {
        CreateMap<CreateVendingCommand, Vending>();
        CreateMap<Vending, VendingView>();
    }
}