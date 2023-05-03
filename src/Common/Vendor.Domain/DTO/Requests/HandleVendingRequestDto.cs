namespace Vendor.Domain.DTO.Requests;

public class HandleVendingRequestDto
{
    public string MaintainerId { get; set; }
    public int VendingId { get; set; }
    public TimeSpan Duration { get; set; }
}