using AutoMapper;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Api.CQRS.Commands;
using Vendor.Services.Machines.Domain.AggregateModel.HandleAggregate;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.MappingProfiles;

public class MachineProfiles : Profile
{
    public MachineProfiles()
    {
        CreateMap<CreateVendingCommand, Vending>();
        CreateMap<Vending, VendingView>();
        CreateMap<Spiral, SpiralView>();
        CreateMap<Handle, HandleView>();
        CreateMap<CreateVendingRequestDto, CreateVendingCommand>();
    }
}