using AutoMapper;
using Vendor.Services.Products.Api.CQRS.Commands;
using Vendor.Services.Products.Api.CQRS.Queries;
using Vendor.Services.Products.Domain.DTO;
using Vendor.Services.Products.DTO;

namespace Vendor.Services.Products.Api.MappingProfiles;

public class ProductsApiMappingProfile : Profile
{
    public ProductsApiMappingProfile()
    {
        CreateMap<CreateProductDto, CreateProductCommand>();
        CreateMap<QueryProductsDto, QueryProductById>();
        CreateMap<QueryProductsByNameDto, QueryProductsByMatchingName>();
    }
}