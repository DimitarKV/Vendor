using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Domain.AggregateModel.HandleAggregate;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;
using Vendor.Services.Machines.Infrastructure;
using Vendor.Services.Machines.Infrastructure.Repositories;

namespace Vendor.Services.Machines.Api;

public static class DataExtensions
{
    public static void AddPersistence(this WebApplicationBuilder builder, string stringName = "Database")
    {
        var connectionStringBuilder =
            new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("Database"));
        connectionStringBuilder.UserID = builder.Configuration["ConnectionStrings:DbUser"];
        connectionStringBuilder.Password = builder.Configuration["ConnectionStrings:DbPassword"];
        builder.Services.AddDbContext<MachineDbContext>(o => o.UseSqlServer(connectionStringBuilder.ConnectionString));
        
        builder.Services.AddTransient<MachineDbContext, MachineDbContext>();
        builder.Services.AddTransient<IMachineRepository, MachineRepository>();
        builder.Services.AddTransient<IHandleRepository, HandleRepository>();
        
        
    }

    public static void EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<MachineDbContext>();
        db!.Database.EnsureCreated();
    }
}