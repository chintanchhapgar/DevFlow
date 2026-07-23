using DevFlow.SharedKernel.Results;
using FluentValidation;
using MediatR;

namespace DevFlow.BuildingBlocks.Api.Behaviors;

/// <summary>
/// MediatR pipeline behavior that validates requests before they reach handlers.
/// Aggregates all validation failures into a single Validation error result.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type, must be a Result.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
            .Distinct()
            .ToList();

        if (failures.Count == 0)
        {
            return await next(cancellationToken);
        }

        return CreateValidationResult<TResponse>(failures);
    }

    private static TResponse CreateValidationResult<T>(List<ValidationError> failures)
        where T : Result
    {
        // Build a combined error description
        var errorDescription = string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));

        var error = AppError.Validation(
            "Validation.Failed",
            errorDescription);

        if (typeof(T) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        // Result<TValue>
        var resultType = typeof(T).GenericTypeArguments[0];

        var failureMethod = typeof(Result)
            .GetMethod(nameof(Result.Failure),
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                [typeof(AppError)])!
            .MakeGenericMethod(resultType);

        return (TResponse)failureMethod.Invoke(null, [error])!;
    }

    private sealed record ValidationError(string PropertyName, string ErrorMessage);
}
