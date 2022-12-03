using FluentValidation;
using MediatR;
using Vendor.Domain.Types;

namespace Vendor.Application.Behaviours;

public class ValidationBehaviours<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviours(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        // var failures = _validators
        //     .Select(v => v.Validate(context))
        //     .SelectMany(result => result.Errors)
        //     .Where(f => f != null)
        //     .ToList();

        var failuresAsync = _validators
            .Select(async v => (await v.ValidateAsync(context, cancellationToken)))
            .SelectMany(r => r.Result.Errors)
            .Where(f => f != null)
            .ToList();
        
        if (failuresAsync.Count != 0)
        {
            var responseType = typeof(TResponse);

            if (responseType.IsGenericType)
            {
                var resultType = responseType.GetGenericArguments()[0];
                var invalidResponseType = typeof(ApiResponse<>).MakeGenericType(resultType);

                var invalidResponse = Activator.CreateInstance(invalidResponseType, null, "Invalid state of the data",
                    failuresAsync.Select(s => s.ErrorMessage).ToList()) as TResponse;

                return invalidResponse!;
            }
            return (Activator.CreateInstance(typeof(ApiResponse), "Invalid state of the data", failuresAsync.Select(s => s.ErrorMessage).ToList()) as TResponse)!;
        }

        return await next();
    }
}