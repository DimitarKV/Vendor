// using CloudinaryDotNet;

using System;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendor.Services.Products.Data.Persistence;
using Vendor.Services.Products.Data.Persistence.Interface;

namespace Vendor.Services.Products.Data.Extensions;

public static class DataExtensions
{
    public static void AddPersistence(this WebApplicationBuilder builder, string stringName = "Database")
    {
        var connectionString = builder.Configuration.GetConnectionString(stringName);
        builder.Services.AddDbContext<ProductsDbContext>(o => o.UseSqlServer(connectionString));
        builder.Services.AddTransient<IProductsDbContext, ProductsDbContext>();
        builder.Services.AddTransient<ProductsDbContext, ProductsDbContext>();
        builder.Services.AddSingleton<Cloudinary, Cloudinary>(sp => new Cloudinary(
            new Account(
                Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
                Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
                Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET"))));
    }

    public static void EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<ProductsDbContext>();
        db!.Database.EnsureCreated();
    }
}