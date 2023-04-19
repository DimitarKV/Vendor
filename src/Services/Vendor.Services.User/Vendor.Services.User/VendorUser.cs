using Microsoft.AspNetCore.Identity;

namespace Vendor.Services.User;

public class VendorUser : IdentityUser
{
    public DateTime CreatedOn { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    public VendorUser()
    {
        CreatedOn = DateTime.Now;
        UpdatedOn = DateTime.Now;
    }
    
    
}