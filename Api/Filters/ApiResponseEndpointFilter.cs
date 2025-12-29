using Api.Models;
using Api.Models.Api;
using Core.Interfaces;

namespace Api.Filters;

public class ApiResponseEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var result = await next(context);

        var response = new ApiResponse { Message = "Succeeded" };

        int statusCode = StatusCodes.Status200OK;

        if (result is not IResult || result is IApiResponse)
            return result;

        if (result is IStatusCodeHttpResult statusCodeResult)
        {
            statusCode = statusCodeResult.StatusCode ?? StatusCodes.Status200OK;
        }

        if (result is IValueHttpResult httpValueResult)
        {
            if (httpValueResult.Value is Error error)
            {
                response.Message = error.Code;
            }
            else
            {
                response.Data = httpValueResult.Value;
            }
        }

        return TypedResults.Json(response, statusCode: statusCode);
    }
}
