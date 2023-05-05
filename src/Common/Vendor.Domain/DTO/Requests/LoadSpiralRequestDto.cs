namespace Vendor.Domain.DTO.Requests;

public class LoadSpiralRequestDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Loads { get; set; }
    public Decimal Price { get; set; }
}