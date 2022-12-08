using AutoMapper;
using Vendor.Services.Machines.Commands.CreateVendorCommand;
using Vendor.Services.Machines.Commands.LoadSpiralCommand;
using Vendor.Services.Machines.Commands.VendingDropCommand;
using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.DTO;
using Vendor.Services.Machines.Views;

namespace Vendor.Services.Machines.MappingProfiles;

public class MachineProfiles : Profile
{
    public MachineProfiles()
    {
        CreateMap<CreateVendingCommand, Vending>();
        CreateMap<Vending, VendingView>();
        CreateMap<CreateVendingDto, CreateVendingCommand>();
        CreateMap<VendingDropDto, VendingDropCommand>();
        CreateMap<LoadSpiralDto, LoadSpiralCommand>();
    }
}