using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Vendor.Services.Products.Domain.AggregateModel.ProductAggregate;
using Vendor.Services.Products.Repositories;

namespace Vendor.Services.Products.Api;

public static class DataExtensions
{
    public static void AddPersistence(this WebApplicationBuilder builder, string stringName = "Database")
    {
        var connectionStringBuilder =
            new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("Database"));
        connectionStringBuilder.UserID = builder.Configuration["ConnectionStrings:DbUser"];
        connectionStringBuilder.Password = builder.Configuration["ConnectionStrings:DbPassword"];
        builder.Services.AddDbContext<ProductsDbContext>(o => o.UseSqlServer(connectionStringBuilder.ConnectionString));
        
        builder.Services.AddTransient<ProductsDbContext, ProductsDbContext>();
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
    }

    public static void EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<ProductsDbContext>();
        db!.Database.EnsureCreated();
    }
}