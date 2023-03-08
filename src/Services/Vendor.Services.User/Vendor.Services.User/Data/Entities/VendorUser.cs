using Microsoft.AspNetCore.Identity;
using Vendor.Domain.Entities;

namespace Vendor.Services.User.Data.Entities;

public class VendorUser : IdentityUser, IEntity<string>
{
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public VendorUser()
    {
        
    }
}