using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.Commands.Cloudinary;
using Vendor.Domain.DTO;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Services.Products.Api.CQRS.Commands;
using Vendor.Services.Products.Api.CQRS.Queries;
using Vendor.Services.Products.Api.DTO;

namespace Vendor.Services.Products.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ProductsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    public async Task<IActionResult> Create([FromForm] CreateProductRequestDto dto)
    {
        var product = (await _mediator.Send(new QueryProductByExactName(dto.Name))).Result;
        if (product is not null)
            return BadRequest(new ApiResponse("Error!", new[] { "A product with name " + dto.Name + " already exists!" }));

        var uploadImageCommand = _mapper.Map<UploadImageCommand>(dto);
        var uploadResult = await _mediator.Send(uploadImageCommand);
        if (!uploadResult.IsValid)
        {
            return BadRequest(uploadResult);
        }

        var createProductCommand = _mapper.Map<CreateProductCommand>(dto);
        createProductCommand.ImageUrl = uploadResult.Result;

        var createProductResult = await _mediator.Send(createProductCommand);

        if (!createProductResult.IsValid)
            return BadRequest(createProductResult);

        return Ok(createProductResult);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    [Route("/[controller]/[action]/{id}")]
    public async Task<IActionResult> Query(int id)
    {
        var query = new QueryProductById() { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsValid)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Maintainer,Admin")]
    [Route("/[controller]/[action]/{name?}")]
    public async Task<IActionResult> QueryMatching(string name = "")
    {
        var query = new QueryProductsByMatchingName(name);
        var result = await _mediator.Send(query);

        if (!result.IsValid)
            return BadRequest(result);

        return Ok(result);
    }
}