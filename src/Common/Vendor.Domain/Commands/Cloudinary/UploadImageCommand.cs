﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Vendor.Domain.Types;

namespace Vendor.Domain.Commands.UploadImageCommand;

public class UploadImageCommand : IRequest<ApiResponse<string>>
{
    public string Name { get; set; }
    public IFormFile Image { get; set; }
}

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, ApiResponse<string>>
{
    private readonly Cloudinary _cloudinary;

    public UploadImageCommandHandler(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<ApiResponse<string>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(request.Image.FileName, request.Image.OpenReadStream()),
            PublicId = request.Name.Replace(" ", "")
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error is not null)
        {
            return new ApiResponse<string>("", "Error", new []{uploadResult.Error.Message});
        }

        return new ApiResponse<string>(uploadResult.Uri.ToString(), "Successfully uploaded image to cloudinary");
    }
}
