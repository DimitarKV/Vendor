using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.Commands.UploadImageCommand;
using Vendor.Domain.Types;
using Vendor.Services.Products.Commands.CreateProductCommand;
using Vendor.Services.Products.DTO;
using Vendor.Services.Products.Queries;

namespace Vendor.Services.Products.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    private readonly Cloudinary _cloudinary;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ProductsController(Cloudinary cloudinary, IMapper mapper, IMediator mediator)
    {
        _cloudinary = cloudinary;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm]CreateProductDto dto)
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
    public async Task<IActionResult> QueryProduct(int id)
    {
        var query = new QueryProductById() { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsValid)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> QueryMatchingProducts(string name)
    {
        var query = new QueryProductsByMatchingName() { Name = name };
        var result = await _mediator.Send(query);

        if (!result.IsValid)
            return BadRequest(result);

        return Ok(result);
    }
}