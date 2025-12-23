using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Services;
using Shared.Services.Interfaces;

namespace Shared
{
    public static class SharedDependencyInjection
    {
        public static IServiceCollection AddSharedEmailService(
            this IServiceCollection services,
            IHostEnvironment env,
            bool useDev = false
        )
        {
            if (env.IsDevelopment() && useDev)
                services.AddScoped<IEmailService, DevEmailService>();
            else
                services.AddScoped<IEmailService, SmtpEmailService>();

            return services;
        }

        public static IServiceCollection AddSharedTemplateService(
            this IServiceCollection services
        ) => services.AddScoped<ITemplateService, TemplateService>();

        public static IServiceCollection AddSharedUriService(this IServiceCollection services)
        {
            return services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
        }
    }
}
