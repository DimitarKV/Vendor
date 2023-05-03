using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vendor.Domain.DTO;
using Vendor.Domain.Types;
using Vendor.Domain.Views;

namespace Vendor.Services.User.Api.CQRS.Queries;

public class QueryUsers : IRequest<ApiResponse<List<UserView>>>
{
    public int Page { get; set; }
    public int PerPage { get; set; }
}

public class QueryUsersHandler : IRequestHandler<QueryUsers, ApiResponse<List<UserView>>>
{
    private readonly UserManager<VendorUser> _userManager;
    private readonly IMapper _mapper;

    public QueryUsersHandler(UserManager<VendorUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<UserView>>> Handle(QueryUsers request, CancellationToken cancellationToken)
    {
        var usersFiltered = (await _userManager.Users
                .Skip(request.Page * request.PerPage)
                .Take(request.PerPage)
                .ToListAsync(cancellationToken: cancellationToken)
            )
            .Select(u =>
            {
                var dto = _mapper.Map<UserView>(u);
                var userRoleClaim = _userManager.GetClaimsAsync(u).Result.SingleOrDefault(c => c.Type == ClaimTypes.Role); 
                dto.Role = userRoleClaim is null ? "N/A" : userRoleClaim.Value;
                return dto;
            })
            .ToList();

        return new ApiResponse<List<UserView>>(usersFiltered);
    }
}

public class QueryUsersValidator : AbstractValidator<QueryUsers>
{
    public QueryUsersValidator()
    {
        RuleFor(cmd => cmd)
            .Must(cmd => cmd.Page >= 0)
            .WithMessage("Page can't be negative!")
            .WithErrorCode("401")
            .Must(cmd => cmd.PerPage >= 0)
            .WithMessage("Per page can't be negative!")
            .WithErrorCode("401");
    }
}