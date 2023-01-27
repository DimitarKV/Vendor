using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.Identity;

public interface IIdentityService
{
    public Task<VendorUser?> FindByNameAsync(string userName);
    public Task<IdentityResult> AddClaimAsync(VendorUser user, Claim claim);
    public Task<IList<Claim>> GetClaimsAsync(VendorUser user);
    public Task<IdentityResult> CreateAsync(VendorUser user, string password);
    public Task<IdentityResult> DeleteAsync(VendorUser user);
    public Task<IdentityResult> ChangePasswordAsync(VendorUser user, string oldPassword, string newPassword);
    public Task<IdentityResult> UpdateAsync(VendorUser user);
    public Task<bool> CheckPasswordAsync(VendorUser user, string password);
    public Task<string> GenerateEmailConfirmationTokenAsync(VendorUser vendorUser);
    public Task<IdentityResult> ConfirmEmailAsync(VendorUser user, string token);
}