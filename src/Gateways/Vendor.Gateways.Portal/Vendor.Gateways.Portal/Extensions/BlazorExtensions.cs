using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Vendor.Gateways.Portal.Middleware;
using Vendor.Gateways.Portal.Providers;
using Vendor.Gateways.Portal.Services.Authentication;
using Vendor.Gateways.Portal.Services.Maintainer;
using Vendor.Gateways.Portal.Services.Product;
using Vendor.Gateways.Portal.Services.User;
using Vendor.Gateways.Portal.Wrappers.HttpClientWrapper;

namespace Vendor.Gateways.Portal.Extensions;

public static class BlazorExtensions
{
    public static void AddHttpClients(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddHttpClient<IMaintainerService, MaintainerService>(
            builder.Configuration["Services:Machines:Client"]!,
            client => { client.BaseAddress = new Uri(builder.Configuration["Services:Machines:Endpoint"]!); });

        services.AddHttpClient<IAuthenticationService, AuthenticationService>(
            builder.Configuration["Services:Users:Client"]!,
            client => { client.BaseAddress = new Uri(builder.Configuration["Services:Users:Endpoint"]!); });

        services.AddHttpClient<IAuthenticationService, AuthenticationService>(
            builder.Configuration["Services:Products:Client"]!,
            client => { client.BaseAddress = new Uri(builder.Configuration["Services:Products:Endpoint"]!); });
    }

    public static void AddBlazorExtensions(this WebApplicationBuilder builder, Assembly[] assemblies)
    {
        var services = builder.Services;
        services.AddAutoMapper(assemblies);

        services.AddScoped<IMaintainerService, MaintainerService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<TokenAuthenticationStateProvider, TokenAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
        services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
    }
}