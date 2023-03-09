using System.ComponentModel.DataAnnotations;

namespace Vendor.Gateways.Portal.DTO;

public class RegisterUserFormData
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The passwords don't match!")]
    public string RepeatPassword { get; set; }
}