using Core.Entities;
using Core.Enums;

namespace Api.Extensions;

public static class MinimalApiExtensions
{
    extension<TBuild>(TBuild builder) where TBuild : IEndpointConventionBuilder
    {
        public TBuild RequireRoles(RoleType role)
        {
            return builder.RequireAuthorization(p => p.RequireRole(role.ToString()));
        }

        public TBuild RequireRoles(params RoleType[] roles)
        {
            return builder.RequireAuthorization(p => p.RequireRole(roles.Select(r => r.ToString())));
        }
    }
}