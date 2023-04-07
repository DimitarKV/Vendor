using Microsoft.AspNetCore.Http;

namespace Vendor.Domain.DTO;

public class CreateVendingDto
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public int Spirals { get; set; }
    public IFormFile Image { get; set; }
}