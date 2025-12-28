using Api.Interfaces;

namespace Api.Features.Accounts;

public class AccountsEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/accounts").WithTags("Accounts");

        authGroup.MapGet("/{slug}", AccountInfoBySlug.Handler);
    }
}
