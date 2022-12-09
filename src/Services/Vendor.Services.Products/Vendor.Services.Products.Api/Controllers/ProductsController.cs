using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.Entities;
using Vendor.Domain.Types;

namespace Vendor.Services.Products.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProduct(int id)
    {
        return Ok(new ApiResponse<Product>(new Product() {ImageUrl = "hhhhh", Name = "name", Id = id}));
    }
}