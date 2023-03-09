using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendor.Services.Machines.Data.Persistence;
using Vendor.Services.Machines.Data.Persistence.Interface;
using Vendor.Services.Machines.Data.Repositories;
using Vendor.Services.Machines.Data.Repositories.Interfaces;

namespace Vendor.Services.Machines.Data.Extensions;

public static class DataExtensions
{
    public static void AddPersistence(this WebApplicationBuilder builder, string stringName = "Database")
    {
        var connectionStringBuilder =
            new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("Database"));
        connectionStringBuilder.UserID = builder.Configuration["ConnectionStrings:DbUser"];
        connectionStringBuilder.Password = builder.Configuration["ConnectionStrings:DbPassword"];
        builder.Services.AddDbContext<MachineDbContext>(o => o.UseSqlServer(connectionStringBuilder.ConnectionString));
        
        builder.Services.AddTransient<IMachineDbContext, MachineDbContext>();
        builder.Services.AddTransient<MachineDbContext, MachineDbContext>();
        builder.Services.AddTransient<IVendingRepository, VendingRepository>();
    }

    public static void EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<MachineDbContext>();
        db!.Database.EnsureCreated();
    }
}