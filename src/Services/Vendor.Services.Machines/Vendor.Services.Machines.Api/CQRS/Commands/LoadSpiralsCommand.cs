﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Vendor.Domain.DTO.Requests;
using Vendor.Domain.Types;
using Vendor.Domain.Views;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

namespace Vendor.Services.Machines.Api.CQRS.Commands;

public class LoadSpiralsCommand : IRequest<ApiResponse<List<SpiralView>>>
{
    public List<LoadSpiralRequestDto> Spirals { get; set; }
}

public class LoadSpiralsCommandHandler : IRequestHandler<LoadSpiralsCommand, ApiResponse<List<SpiralView>>>
{
    private readonly IMapper _mapper;
    private readonly IMachineRepository _repository;

    public LoadSpiralsCommandHandler(IMapper mapper, IMachineRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResponse<List<SpiralView>>> Handle(LoadSpiralsCommand request,
        CancellationToken cancellationToken)
    {
        foreach (var spiralRequestDto in request.Spirals)
            await _repository.LoadSpiralAsync(spiralRequestDto.Id, spiralRequestDto.ProductId, spiralRequestDto.Loads,
                spiralRequestDto.Price);

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var spirals = new List<SpiralView>();

        foreach (var spiralRequestDto in request.Spirals)
            spirals.Add(_mapper.Map<SpiralView>(await _repository.GetSpiralByIdAsync(spiralRequestDto.Id)));


        return new ApiResponse<List<SpiralView>>(spirals, "Spirals loaded");
    }
}

public class LoadSpiralsCommandValidator : AbstractValidator<LoadSpiralsCommand>
{
    //TODO: add more validations for price and loads
    public LoadSpiralsCommandValidator(IMachineRepository repository)
    {
        RuleFor(v => v)

            .MustAsync(async (ls, _) =>
            {
                foreach (var spiral in ls.Spirals)
                {
                    if ((await repository.GetSpiralByIdAsync(spiral.Id)) is null)
                        return false;
                }

                return true;
            })
            .WithMessage("No such spiral in the the database")
            .WithErrorCode("400");
    }
}