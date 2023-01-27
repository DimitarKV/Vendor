using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<VendorUser> _userManager;

    public IdentityService(UserManager<VendorUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<VendorUser?> FindByNameAsync(string name)
    {
        return _userManager.FindByNameAsync(name);
    }

    public Task<IdentityResult> AddClaimAsync(VendorUser user, Claim claim)
    {
        return _userManager.AddClaimAsync(user, claim);
    }

    public Task<IList<Claim>> GetClaimsAsync(VendorUser user)
    {
        return _userManager.GetClaimsAsync(user);
    }

    public Task<IdentityResult> CreateAsync(VendorUser user, string password)
    {
        return _userManager.CreateAsync(user, password);
    }

    public Task<IdentityResult> DeleteAsync(VendorUser user)
    {
        return _userManager.DeleteAsync(user);
    }

    public Task<IdentityResult> ChangePasswordAsync(VendorUser user, string oldPassword, string newPassword)
    {
        return _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    }

    public Task<IdentityResult> UpdateAsync(VendorUser user)
    {
        return _userManager.UpdateAsync(user);
    }

    public Task<bool> CheckPasswordAsync(VendorUser user, string password)
    {
        return _userManager.CheckPasswordAsync(user, password);
    }

    public Task<string> GenerateEmailConfirmationTokenAsync(VendorUser vendorUser)
    {
        return _userManager.GenerateEmailConfirmationTokenAsync(vendorUser);
    }

    public Task<IdentityResult> ConfirmEmailAsync(VendorUser user, string token)
    {
        return _userManager.ConfirmEmailAsync(user, token);
    }
}