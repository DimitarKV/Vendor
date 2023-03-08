using System.ComponentModel.DataAnnotations;

namespace Vendor.Gateways.Portal.DTO;

public class RegisterUserDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}