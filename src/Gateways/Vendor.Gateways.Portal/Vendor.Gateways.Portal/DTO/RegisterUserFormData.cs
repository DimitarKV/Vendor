using System.ComponentModel.DataAnnotations;

namespace Vendor.Gateways.Portal.DTO;

public class RegisterUserFormData
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string RepeatPassword { get; set; }
}