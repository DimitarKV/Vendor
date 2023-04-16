using AutoMapper;
using Vendor.Domain.Commands.Cloudinary;
using Vendor.Domain.Views;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;
using Vendor.Services.Products.Domain.DTO;
using Vendor.Services.Products.DTO;

namespace Vendor.Services.Products.Domain.MappingProfiles;

public class ProductProfiles : Profile
{
    public ProductProfiles()
    {
        CreateMap<Product, ProductView>();
        CreateMap<CreateProductDto, UploadImageCommand>();
    }
}