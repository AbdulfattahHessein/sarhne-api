using System.Collections;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Models.Api;

public abstract class ApiEndpoint
{
    public static NoContent NoContent()
    {
        return TypedResults.NoContent();
    }

    public static Ok<SuccessResponse<T>> Ok<T>(T value, string message = "Succeeded")
    {
        return TypedResults.Ok(SuccessResponse<T>.Success(value, message));
    }

    public static Ok<ApiResponse> Ok(string message = "Succeeded")
    {
        return TypedResults.Ok(ApiResponse.Success(message));
    }

    public static BadRequest<ApiResponse> BadRequest(string message)
    {
        return TypedResults.BadRequest(ApiResponse.Failure(message));
    }

    public static Ok<PaginatedResponse<T>> Ok<T>(
        T value,
        int totalCount,
        int pageNumber = 1,
        int pageSize = 10
    )
        where T : IEnumerable =>
        TypedResults.Ok(PaginatedResponse<T>.Success(value, totalCount, pageNumber, pageSize));

    public static Created<SuccessResponse<T>> Created<T>(
        string uri,
        T value,
        string message = "Created"
    )
    {
        return TypedResults.Created(uri, SuccessResponse<T>.Success(value, message));
    }

    public static Created<SuccessResponse<BaseResponse>> Created(
        string uri,
        Guid value,
        string message = "Created"
    )
    {
        return Created(uri, value, message);
    }

    public static Created<ApiResponse> Created(string uri, string message = "Created")
    {
        return TypedResults.Created(uri, ApiResponse.Success(message));
    }

    public static NotFound<ApiResponse> NotFound(string message = nameof(Errors.NotFound))
    {
        return TypedResults.NotFound(ApiResponse.Failure(message));
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
