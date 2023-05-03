using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.Commands.Cloudinary;
using Vendor.Domain.DTO.Requests;
using Vendor.Services.Machines.Api.CQRS.Commands;
using Vendor.Services.Machines.Api.CQRS.Queries;

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
    public async Task<IActionResult> Create(CreateVendingRequestDto request)
    {
        var createCommand = _mapper.Map<CreateVendingCommand>(request);

        var createResult = await _mediator.Send(createCommand);

        if (!createResult.IsValid)
            return BadRequest(createResult);

        return Ok(createResult);
    }

    [Route("/[controller]/[action]/{machineId}")]
    public async Task<IActionResult> SetImage([FromForm] IFormFile image, [FromRoute] int machineId)
    {
        //TODO: Fix the bug where an image is uploaded even if the machine id doesn't exist
        var uploadResult = await _mediator.Send(new UploadImageCommand()
            { Image = image, Name = Guid.NewGuid().ToString() });
        if (!uploadResult.IsValid)
            return BadRequest(uploadResult);

        var setImageResult = await _mediator.Send(new SetMachineImageCommand()
            { ImageUrl = uploadResult.Result, MachineId = machineId });
        if (!setImageResult.IsValid)
            return BadRequest(setImageResult);

        return Ok(setImageResult);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    public async Task<IActionResult> Load(LoadSpiralRequestDto request)
    {
        var command = _mapper.Map<LoadSpiralCommand>(request);

        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Machine,Maintainer,Admin")]
    public async Task<IActionResult> Drop(VendingDropRequestDto request)
    {
        var command = _mapper.Map<VendingDropCommand>(request);

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

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    public async Task<IActionResult> HandleMachine(HandleVendingRequestDto request)
    {
        var command = _mapper.Map<HandleVendingCommand>(request);

        var result = await _mediator.Send(command);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    [Route("/[controller]/[action]/{id}")]
    public async Task<IActionResult> Fetch(int id)
    {
        var query = new QueryVendingById() { Id = id };
        var result = await _mediator.Send(query);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    [Route("/[controller]/[action]/{id}")]
    public async Task<IActionResult> QueryMissingProducts(int id)
    {
        var query = new QueryMissingProducts() { MachineId = id };
        var result = await _mediator.Send(query);

        if (result.IsValid)
            return Ok(result);
        return BadRequest(result);
    }
}