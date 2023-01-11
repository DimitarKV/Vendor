using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Vendor.Gateways.Portal.Services;

namespace Vendor.Gateways.Portal.Extensions;

public static class BlazorExtensions
{
    public static void AddHttpClients(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services.AddHttpClient<IMaintainerService, MaintainerService>(builder.Configuration["Services:Machines:Client"]!, client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Services:Machines:Endpoint"]!);
        });
        
        services.AddScoped<IMaintainerService, MaintainerService>();
    }
}