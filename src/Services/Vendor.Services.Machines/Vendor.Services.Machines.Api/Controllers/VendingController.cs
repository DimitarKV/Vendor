using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.DTO;
using Vendor.Services.Machines.Api.CQRS.Commands;
using Vendor.Services.Machines.Api.CQRS.Queries;
using Vendor.Services.Machines.DTO;

namespace Vendor.Services.Machines.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class VendingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public VendingController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    public async Task<IActionResult> Create(CreateVendingCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    public async Task<IActionResult> Load(LoadSpiralCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Machine,Maintainer,Admin")]
    public async Task<IActionResult> Drop(VendingDropCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    public async Task<IActionResult> QueryEmpty()
    {
        var result = await _mediator.Send(new QueryEmptyVendings());

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }
}