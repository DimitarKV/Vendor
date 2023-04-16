using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Vendor.Services.Products.Domain.DTO;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public IFormFile Image { get; set; }
}