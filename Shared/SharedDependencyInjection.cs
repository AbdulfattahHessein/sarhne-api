using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Models.Email;
using Shared.Services;
using Shared.Services.Interfaces;

namespace Shared;

public static class SharedDependencyInjection
{
    public static IServiceCollection AddSharedEmailService(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env,
        bool useDev = false
    )
    {
        if (env.IsDevelopment() && useDev)
            services.AddScoped<IEmailService, DevEmailService>();
        else
            services.AddScoped<IEmailService, EmailService>();

        return services.AddEmailConfiguration(configuration);
    }

    public static IServiceCollection AddSharedTemplateService(this IServiceCollection services) =>
        services.AddScoped<ITemplateService, TemplateService>();

    public static IServiceCollection AddSharedClaimService(this IServiceCollection services) =>
        services.AddScoped<IClaimService, ClaimService>();

    public static IServiceCollection AddSharedSmsService(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env
    )
    {
        var useDevSms = configuration.GetValue<bool>("AppSettings:UseDevSms");
        if (env.IsDevelopment() || useDevSms)
            services.AddScoped<ISmsService, DevSmsService>();
        else
            services.AddScoped<ISmsService, SmsService>();

        return services;
    }

    private static IServiceCollection AddEmailConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var emailConfig =
            configuration.GetSection("SmtpSettings").Get<SmtpSettings>()
            ?? throw new JsonException("SmtpSettings section not found in appsettings.json");

        return services.AddSingleton(emailConfig);
    }

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
