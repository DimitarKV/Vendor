using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace Vendor.Domain.DTO;

public class CreateVendingDto
{
    // [Required]
    public string Title { get; set; }
    // [Required]
    public Double Latitude { get; set; }
    // [Required]
    public Double Longitude { get; set; }
    // [Range(typeof(int), "1", "500", ErrorMessage = "There must be at least one spiral in the machine")]
    public int Spirals { get; set; }
    
    // [Required]
    public IBrowserFile Image { get; set; }
}