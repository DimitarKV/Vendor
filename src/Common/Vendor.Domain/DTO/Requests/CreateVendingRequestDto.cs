using Microsoft.AspNetCore.Http;

namespace Vendor.Domain.DTO.Requests;

public class CreateVendingRequestDto
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public int Spirals { get; set; }
}