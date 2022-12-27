using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Services.Machines.DTO;
using Vendor.Services.Machines.Queries;

namespace Vendor.Services.Machines.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class SpiralController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SpiralController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Query(QuerySpiralDto dto)
    {
        var command = _mapper.Map<QuerySpiral>(dto);
        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }
}