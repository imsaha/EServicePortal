using EServicePortal.Application.Common.Wrappers;
using FluentValidation;
using FluentValidation.Results;
using MediatR.Pipeline;

namespace EServicePortal.Application.Common.Behaviours;

internal class ValidationBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : ICommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return;
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults.SelectMany(x => x.Errors).Distinct();
        var validationFailures = failures as ValidationFailure[] ?? failures.ToArray();
        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }
    }
}
