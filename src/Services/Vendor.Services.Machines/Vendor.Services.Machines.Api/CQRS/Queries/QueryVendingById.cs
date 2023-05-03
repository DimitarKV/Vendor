using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Queries;

public class QueryVendingById : IRequest<ApiResponse<VendingView>>
{
    public int Id { get; set; }
}

public class QueryVendingByIdHandler : IRequestHandler<QueryVendingById, ApiResponse<VendingView>>
{
    private readonly IMachineRepository _repository;
    private readonly IMapper _mapper;

    public QueryVendingByIdHandler(IMachineRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<VendingView>> Handle(QueryVendingById request, CancellationToken cancellationToken)
    {
        var vending = await _repository.GetVendingByIdAsync(request.Id);
        var vendingView = _mapper.Map<VendingView>(vending);
        return new ApiResponse<VendingView>(vendingView);
    }
}

public class QueryVendingByIdValidator : AbstractValidator<QueryVendingById>
{
    public QueryVendingByIdValidator(IMachineRepository repository)
    {
        RuleFor(q => q)
            .MustAsync(async (q, _) => (await repository.GetVendingByIdAsync(q.Id)) is not null)
            .WithMessage("No such machine in the database!")
            .WithErrorCode("400");
    }
}