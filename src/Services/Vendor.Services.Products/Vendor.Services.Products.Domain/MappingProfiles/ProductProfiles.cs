using AutoMapper;
using Vendor.Domain.Commands.Cloudinary;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Views;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;

namespace Vendor.Services.Products.Domain.MappingProfiles;

public class ProductProfiles : Profile
{
    public ProductProfiles()
    {
        CreateMap<Product, ProductView>();
        CreateMap<CreateProductRequestDto, UploadImageCommand>();
    }
}