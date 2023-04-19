using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Vendor.Services.User.Api;

public static class DataExtensions
{
    public static void AddPersistence(this WebApplicationBuilder builder, string name = "Database")
    {
        var connectionStringBuilder =
            new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("Database"));
        connectionStringBuilder.UserID = builder.Configuration["ConnectionStrings:DbUser"];
        connectionStringBuilder.Password = builder.Configuration["ConnectionStrings:DbPassword"];
        builder.Services.AddDbContext<UserDbContext>(o => o.UseSqlServer(connectionStringBuilder.ConnectionString));
    }

    public static void EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<UserDbContext>();
        db!.Database.EnsureCreated();
    }
}