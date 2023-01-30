﻿using AutoMapper;
using Vendor.Services.User.Commands.User;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, VendorUser>();
    }
}