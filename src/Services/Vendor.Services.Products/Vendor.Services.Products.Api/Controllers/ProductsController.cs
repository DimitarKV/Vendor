// using CloudinaryDotNet;
// using CloudinaryDotNet.Actions;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Vendor.Domain.Entities;
using Vendor.Domain.Types;

namespace Vendor.Services.Products.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    private readonly Cloudinary _cloudinary;
    
    public ProductsController(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    [HttpGet]
    public async Task<IActionResult> Product(int id)
    {
        var uploadParams = new ImageUploadParams(){
            File = new FileDescription(@"https://upload.wikimedia.org/wikipedia/commons/a/ae/Olympic_flag.jpg"),
            PublicId = "olympic_flag"};
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return Ok(new ApiResponse<Product>(new Product() {ImageUrl = "hhhhh", Name = "name", Id = id}));
    }
}