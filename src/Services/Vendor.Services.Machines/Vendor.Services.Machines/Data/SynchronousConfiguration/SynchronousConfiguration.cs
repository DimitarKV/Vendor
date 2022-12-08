using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Vendor.Services.Machines.Data.Repositories;
using Vendor.Services.Machines.Data.Repositories.Interfaces;

namespace Vendor.Services.Machines.Data.SynchronousConfiguration;

public static class SynchronousConfiguration
{
    public static void AddHttpClients(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddHttpClient<IProductRepository, ProductRepository>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ProductsUrl"]!);
        });
    }
}