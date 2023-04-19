using AutoMapper;
using Vendor.Domain.Views;
using Vendor.Services.User.Api.CQRS.Commands.User;

namespace Vendor.Services.User.Api.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, VendorUser>();
        CreateMap<VendorUser, UserView>();
    }
}