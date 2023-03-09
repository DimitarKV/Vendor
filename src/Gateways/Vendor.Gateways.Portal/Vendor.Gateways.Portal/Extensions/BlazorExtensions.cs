using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Vendor.Gateways.Portal.Services.Authentication;
using Vendor.Gateways.Portal.Services.Maintainer;

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
        
        services.AddHttpClient<IAuthenticationService, AuthenticationService>(builder.Configuration["Services:Users:Client"]!, client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Services:Users:Endpoint"]!);
        });
        
        services.AddScoped<IMaintainerService, MaintainerService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}