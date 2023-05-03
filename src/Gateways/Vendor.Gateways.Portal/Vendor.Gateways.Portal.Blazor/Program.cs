using Vendor.Gateways.Portal.Blazor.MappingProfiles;
using Vendor.Gateways.Portal.Extensions;
using Vendor.Gateways.Portal.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(hub => hub.MaximumReceiveMessageSize = 100 * 1024 * 1024);
builder.Services.AddAuthorization();
builder.AddHttpClients();
builder.AddBlazorExtensions(new[] { typeof(TokenAuthenticationStateProvider).Assembly, typeof(BlazorProfile).Assembly });

var app = builder.Build();

// Configure the HTTP request pipeline. 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();