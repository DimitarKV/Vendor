using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Vendor.Domain.DTO.Requests;

public class CreateProductRequestDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public IFormFile Image { get; set; }
}