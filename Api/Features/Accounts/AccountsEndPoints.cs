using Api.Extensions;
using Api.Interfaces;
using Core.Enums;

namespace Api.Features.Accounts;

public class AccountsEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var accountsGroup = app.MapGroup("/accounts").WithTags("Accounts");

        accountsGroup.MapGet("/{slug}", AccountInfoBySlug.Handler);

        accountsGroup.MapGet("/me", GetMyAccount.Handler).RequireRoles(RoleType.User);
    }
}
