using ErrorOr;
using FluentValidation;
using MediatR;

namespace FurnitureStore.Application.Common.Behaviors;

public class RequestValidationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (validationFailures.Any())
        {
            var validationErrors = validationFailures
                .ConvertAll(failure => Error.Validation(
                    code: failure.PropertyName,
                    description: failure.ErrorMessage));

            return validationErrors.ToTResponse<TResponse>();           
        }

        return await next();
    }
}
