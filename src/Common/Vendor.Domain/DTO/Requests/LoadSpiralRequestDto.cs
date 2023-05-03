namespace Vendor.Domain.DTO.Requests;

public class LoadSpiralRequestDto
{
    public int SpiralId { get; set; }
    public int ProductId { get; set; }
    public int Loads { get; set; }
    public Double Price { get; set; }
}