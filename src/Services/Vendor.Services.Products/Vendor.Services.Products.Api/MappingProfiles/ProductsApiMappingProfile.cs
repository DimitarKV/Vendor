using AutoMapper;
using Vendor.Domain.DTO.Requests;
using Vendor.Services.Products.Api.CQRS.Commands;

namespace Vendor.Services.Products.Api.MappingProfiles;

public class ProductsApiMappingProfile : Profile
{
    public ProductsApiMappingProfile()
    {
        CreateMap<CreateProductRequestDto, CreateProductCommand>();
    }
}