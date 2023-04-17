using AutoMapper;
using Vendor.Domain.Views;
using Vendor.Services.User.CQRS.Commands.User;
using Vendor.Services.User.Data.Entities;

namespace Vendor.Services.User.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, VendorUser>();
        CreateMap<VendorUser, UserView>();
    }
}