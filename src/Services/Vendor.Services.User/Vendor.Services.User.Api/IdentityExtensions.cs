using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Vendor.Services.User.Api.SendGridConfiguration;
using Vendor.Services.User.Api.SendGridConfiguration.Sender;

namespace Vendor.Services.User.Api;

public static class IdentityExtensions
{
    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
        
        var services = builder.Services;
        
        
        services.AddIdentity<VendorUser, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            
            // options.SignIn.RequireConfirmedAccount = true;
        });

        services.AddTransient<IEmailSender, EmailSender>();
        services.Configure<AuthMessageSenderOptions>(builder.Configuration);
    }
}