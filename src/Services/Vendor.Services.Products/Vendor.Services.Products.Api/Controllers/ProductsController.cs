using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.Commands.UploadImageCommand;
using Vendor.Services.Products.Commands;
using Vendor.Services.Products.DTO;
using Vendor.Services.Products.Queries;

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
    public async Task<IActionResult> Create([FromForm]CreateProductDto dto)
    {
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
    public async Task<IActionResult> QueryMatching(QueryProductsByNameDto dto)
    {
        var query = _mapper.Map<QueryProductsByMatchingName>(dto);
        var result = await _mediator.Send(query);

        if (!result.IsValid)
            return BadRequest(result);

        return Ok(result);
    }
}