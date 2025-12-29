using System.Collections;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Models.Api;

public abstract class ApiEndpoint
{
    public static NoContent NoContent()
    {
        return TypedResults.NoContent();
    }

    public static Ok<ApiResponse<T>> Ok<T>(T value, string message = "Succeeded")
    {
        return TypedResults.Ok(ApiResponses.Success(value, message));
    }

    public static Ok<ApiResponse> Ok(string message = "Succeeded")
    {
        return TypedResults.Ok(ApiResponses.Success(message));
    }

    public static BadRequest<ApiResponse> BadRequest(string message)
    {
        return TypedResults.BadRequest(ApiResponses.Failure(message));
    }

    public static Ok<ApiResponse<T>> Ok<T>(
        T value,
        int totalCount,
        int? pageNumber = 1,
        int? pageSize = 10
    )
        where T : IEnumerable =>
        TypedResults.Ok(ApiResponses.Success(value, totalCount, pageNumber ?? 1, pageSize ?? 10));

    public static Created<ApiResponse<T>> Created<T>(
        string uri,
        T value,
        string message = "Created"
    )
    {
        return TypedResults.Created(uri, ApiResponses.Success(value, message));
    }

    public static Created<ApiResponse<BaseResponse>> Created(
        string uri,
        Guid value,
        string message = "Created"
    )
    {
        return Created(uri, value, message);
    }

    public static Created<ApiResponse> Created(string uri, string message = "Created")
    {
        return TypedResults.Created(uri, ApiResponses.Success(message));
    }

    public static NotFound<ApiResponse> NotFound(string message = nameof(Errors.NotFound))
    {
        return TypedResults.NotFound(ApiResponses.Failure(message));
    }

    public static UnauthorizedHttpResult Unauthorized()
    {
        return TypedResults.Unauthorized();
    }

    public static Results<
        NotFound<ApiResponse>,
        BadRequest<ApiResponse>,
        UnprocessableEntity<ApiResponse>,
        UnauthorizedHttpResult
    > Failure(Error error)
    {
        var response = ApiResponses.Failure(error.Code);

        return error switch
        {
            var e when e == Errors.NotFound => TypedResults.NotFound(response),
            var e when e == Errors.Validation => TypedResults.UnprocessableEntity(response),
            var e when e == Errors.Unauthorized => TypedResults.Unauthorized(),
            _ => TypedResults.BadRequest(response),
        };
    }
}
