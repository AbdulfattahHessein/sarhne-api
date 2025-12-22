using Microsoft.AspNetCore.Authentication.Cookies;

namespace Api.DI;

public static class CookieAuthenticationDI
{
    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "ApiAuthCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.Cookie.IsEssential = true; // Bypass GDPR consent if the cookie is strictly for login

                options.Events = new CookieAuthenticationEvents
                {
                    // Triggered when user is NOT logged in
                    OnRedirectToLogin = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(
                            new
                            {
                                status = 401,
                                message = "Unauthorized: Please log in to access this resource.",
                            }
                        );
                    },

                    // Triggered when user is logged in but lacks the required Role/Permission
                    OnRedirectToAccessDenied = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(
                            new
                            {
                                status = 403,
                                message = "Forbidden: You do not have permission to perform this action.",
                            }
                        );
                    },
                };
            });

        return services;
    }

    public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder app)
    {
        return app.UseAuthentication().UseAuthorization();
    }
}
