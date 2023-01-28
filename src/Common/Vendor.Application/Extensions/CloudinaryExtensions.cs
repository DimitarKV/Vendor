using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Vendor.Application.Extensions;

public static class CloudinaryExtensions
{
    public static void AddCloudinary(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Cloudinary, Cloudinary>(sp => new Cloudinary(
            new Account(
                builder.Configuration["Cloudinary:CloudName"],
                builder.Configuration["Cloudinary:APIKey"],
                builder.Configuration["Cloudinary:APISecret"])));
    }
}