using AutoMapper;
using Vendor.Domain.DTO;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Commands;
using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.DTO;
using Vendor.Services.Machines.Queries;
using Vendor.Services.Machines.Views;
using SpiralView = Vendor.Domain.Views.SpiralView;

namespace Vendor.Services.Machines.MappingProfiles;

public class MachineProfiles : Profile
{
    public MachineProfiles()
    {
        CreateMap<CreateVendingCommand, Vending>();

        CreateMap<Vending, VendingView>()
            .ForMember(destinationMember =>
                    destinationMember.Spirals,
                memberOptions =>
                    memberOptions.MapFrom(
                        src => src.Spirals
                            .Select(s => new SpiralView(){ProductId = s.ProductId, Quantity = s.Loads})
                            .ToList()
                    )
            )
            // .ForMember(destinationMember => destinationMember.Banknotes,
            //     memberOptions => 
            //         memberOptions.MapFrom(
            //             src => src.Banknotes))
            ;

        CreateMap<CreateVendingDto, CreateVendingCommand>();
        CreateMap<VendingDropDto, VendingDropCommand>();
        CreateMap<LoadSpiralDto, LoadSpiralCommand>();
        CreateMap<Spiral, Views.SpiralView>();
        CreateMap<QuerySpiralDto, QuerySpiral>();
    }
}