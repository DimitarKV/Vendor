using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.Types;
using Vendor.Services.Machines.Commands.CreateVendorCommand;

namespace Vendor.Services.Machines.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MachineController : ControllerBase
{
    private readonly IMediator _mediator;

    public MachineController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result =
            await _mediator.Send(new CreateVendingCommand() {Title = "My machine", Latitude = 24.0, Longitude = 42.0});

        if (result.IsValid)
            return Ok(result);
        return BadRequest(new ApiResponse(result));
    }
}