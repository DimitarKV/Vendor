using AutoMapper;
using Vendor.Domain.DTO;
using Vendor.Domain.Views;
using Vendor.Services.Machines.AggregateModel.HandleAggregate;
using Vendor.Services.Machines.AggregateModel.MachineAggregate;
using Vendor.Services.Machines.Api.CQRS.Commands;
using Vendor.Services.Machines.Api.CQRS.Queries;
using Vendor.Services.Machines.DTO;

namespace Vendor.Services.Machines.Api.MappingProfiles;

public class MachineProfiles : Profile
{
    public MachineProfiles()
    {
        CreateMap<CreateVendingCommand, Vending>();
        CreateMap<Vending, VendingView>();
        CreateMap<CreateVendingDto, CreateVendingCommand>();
        CreateMap<VendingDropDto, VendingDropCommand>();
        CreateMap<LoadSpiralDto, LoadSpiralCommand>();
        CreateMap<Spiral, SpiralView>();
        CreateMap<QuerySpiralDto, QuerySpiral>();
        CreateMap<Handle, HandleView>();
    }
}