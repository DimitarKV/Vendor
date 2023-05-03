namespace Vendor.Domain.DTO.Requests;

public class ConfirmEmailRequestDto
{
    public string Token { get; set; }
    public string Username { get; set; }
}