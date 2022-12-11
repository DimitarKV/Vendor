using Microsoft.AspNetCore.Http;

namespace Vendor.Services.Products.DTO;

public class CreateProductDto
{
    public string Name { get; set; }
    public IFormFile Image { get; set; }
}