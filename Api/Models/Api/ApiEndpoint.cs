using System.Collections;
using Core;
using Core.Interfaces;

namespace Api.Models.Api;

public class ApiEndpoint
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

    public static IResult NotFound(string message = nameof(Error.NotFound))
    {
        return TypedResults.NotFound(ApiResponse.Failure(message));
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
            var e when e == Error.NotFound => TypedResults.NotFound(response),
            var e when e == Error.Validation => TypedResults.UnprocessableEntity(response),
            var e when e == Error.Unauthorized => TypedResults.Unauthorized(),
            _ => TypedResults.BadRequest(response),
        };
    }
}

public abstract class ApiResultsc
{
    public static IResult NoContent<T>(Result<T> result)
    {
        return HandleResult(result, value => ApiResponse.Success());
    }

    public static IResult ApiResult<T>(Result<T> result)
    {
        return HandleResult(result, value => SuccessResponse<T>.Success(value));
    }

    public static IResult ApiResult<T>(
        Result<T> result,
        int totalCount,
        int pageNumber = 1,
        int pageSize = 10
    )
        where T : IEnumerable =>
        HandleResult(
            result,
            value => PaginatedResponse<T>.Success(value, totalCount, pageNumber, pageSize)
        );

    private static IResult HandleResult<T>(Result<T> result, Func<T, IApiResponse> onSuccess) =>
        result.Match(value => TypedResults.Ok(onSuccess(value)), HandleError);

    private static IResult HandleError(Error error)
    {
        var response = ApiResponse.Failure(error.Code);

        return error switch
        {
            var e when e == Error.NotFound => TypedResults.NotFound(response),
            var e when e == Error.Validation => TypedResults.UnprocessableEntity(response),
            var e when e == Error.Unauthorized => TypedResults.Json(
                response,
                statusCode: StatusCodes.Status401Unauthorized
            ),
            _ => TypedResults.BadRequest(response),
        };
    }
}
