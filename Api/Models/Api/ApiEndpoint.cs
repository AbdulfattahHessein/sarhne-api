using System.Collections;

namespace Api.Models.Api;

public abstract class ApiEndpoint
{
    public static IResult NoContent()
    {
        return TypedResults.NoContent();
    }

    public static IResult Ok<T>(T value, string message = "Succeeded")
    {
        return TypedResults.Ok(SuccessResponse<T>.Success(value, message));
    }

    public static IResult Ok(string message = "Succeeded")
    {
        return TypedResults.Ok(ApiResponse.Success(message));
    }

    public static IResult BadRequest(string message)
    {
        return TypedResults.BadRequest(ApiResponse.Failure(message));
    }

    public static IResult Ok<T>(T value, int totalCount, int pageNumber = 1, int pageSize = 10)
        where T : IEnumerable =>
        TypedResults.Ok(PaginatedResponse<T>.Success(value, totalCount, pageNumber, pageSize));

    public static IResult NotFound(string message = nameof(Errors.NotFound))
    {
        return TypedResults.NotFound(ApiResponse.Failure(message));
    }

    public static IResult Unauthorized()
    {
        return TypedResults.Unauthorized();
    }

    public static IResult Failure(string code)
    {
        var error = new Error(code);

        return Failure(error);
    }

    public static IResult Failure(Error error)
    {
        var response = ApiResponse.Failure(error.Code);

        return error switch
        {
            var e when e == Errors.NotFound => TypedResults.NotFound(response),
            var e when e == Errors.Validation => TypedResults.UnprocessableEntity(response),
            var e when e == Errors.Unauthorized => TypedResults.Unauthorized(),
            _ => TypedResults.BadRequest(response),
        };
    }
}
