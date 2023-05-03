using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Vendor.Domain.DTO.Requests;
using Vendor.Gateways.Portal.DTO;
using Vendor.Gateways.Portal.Models;

namespace Vendor.Gateways.Portal.Blazor.MappingProfiles;

public class BlazorProfile : Profile
{
    public BlazorProfile()
    {
        // CreateMap<IBrowserFile, IFormFile>()
        //     .ForMember(dest => dest,
        //         opt => opt.MapFrom(src =>
        //         {
        //             using var memoryStream = new MemoryStream();
        //             src.OpenReadStream().CopyTo(memoryStream);
        //             return new FormFile(memoryStream, 0, src.Size, src.);
        //         })
        //     );

        CreateMap<CreateVendingModel, CreateVendingRequestDto>();
        CreateMap<RegisterProductModel, RegisterProductDto>();
    }
}