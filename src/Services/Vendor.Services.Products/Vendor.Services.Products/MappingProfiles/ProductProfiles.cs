using AutoMapper;
using Vendor.Domain.Commands.UploadImageCommand;
using Vendor.Domain.Entities;
using Vendor.Domain.Views;
using Vendor.Services.Products.Commands;
using Vendor.Services.Products.DTO;
using Vendor.Services.Products.Queries;

namespace Vendor.Services.Products.MappingProfiles;

public class ProductProfiles : Profile
{
    public ProductProfiles()
    {
        CreateMap<CreateProductDto, CreateProductCommand>();
        CreateMap<Product, ProductView>();
        CreateMap<CreateProductDto, UploadImageCommand>();
        CreateMap<QueryProductsDto, QueryProductById>();
        CreateMap<QueryProductsByNameDto, QueryProductsByMatchingName>();
    }
}