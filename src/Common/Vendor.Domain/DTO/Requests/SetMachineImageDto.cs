using Microsoft.AspNetCore.Components.Forms;

namespace Vendor.Domain.DTO.Requests;

public class SetMachineImageDto
{
    public IBrowserFile Image { get; set; }
    public int MachineId { get; set; }
}