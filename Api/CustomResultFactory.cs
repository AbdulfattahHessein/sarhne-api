using Api.Models.Api;
using FluentValidation.Results;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;

namespace Api;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IResult CreateResult(
        EndpointFilterInvocationContext context,
        ValidationResult validationResult
    )
    {
        var validationProblemErrors = validationResult
            .Errors.Select(e => new { e.PropertyName, e.ErrorCode })
            .GroupBy(k => k.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorCode).ToArray());

        return Results.BadRequest(
            ApiResponses.Failure(validationProblemErrors, "Validation failed")
        );
    }
}
