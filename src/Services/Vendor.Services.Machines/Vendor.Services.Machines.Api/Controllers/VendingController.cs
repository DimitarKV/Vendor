using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.DTO;
using Vendor.Services.Machines.Commands;
using Vendor.Services.Machines.DTO;
using Vendor.Services.Machines.Queries;

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
    public async Task<IActionResult> Create(CreateVendingDto dto)
    {
        var command = _mapper.Map<CreateVendingCommand>(dto);
        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    public async Task<IActionResult> Load(LoadSpiralDto dto)
    {
        var command = _mapper.Map<LoadSpiralCommand>(dto);
        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Machine,Maintainer,Admin")]
    public async Task<IActionResult> Drop(VendingDropDto dto)
    {
        var command = _mapper.Map<VendingDropCommand>(dto);
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