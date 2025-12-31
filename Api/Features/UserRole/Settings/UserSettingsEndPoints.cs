using Api.Extensions;
using Api.Interfaces;
using Core.Enums;

namespace Api.Features.UserRole.Settings;

public class UserSettingsEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var userSettings = app.MapGroup("/user")
            .MapGroup("/settings")
            .WithTags("User Settings")
            .RequireRoles(RoleType.User);

        userSettings.MapPatch("/personal-information", ChangePersonalInformation.Handler);

        userSettings.MapPatch("/email", ChangeEmail.Handler);

        userSettings.MapPatch("/profile-slug", ChangeProfileSlug.Handler);

        userSettings.MapPatch("/password", ChangePassword.Handler);

        userSettings.MapPatch("/privacy", ChangePrivacy.Handler);
    }
}
