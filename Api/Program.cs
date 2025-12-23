using Api.DI;
using Api.Extensions;
using Api.Interfaces;
using Api.Middleware;
using Api.Services.Implementations;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;
using Shared.Services.Interfaces;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.CreateSchemaReferenceId = (json) =>
    {
        if (json.Type.FullName == null)
            return json.Type.Name;

        return json.Type.FullName.Replace("Api.Features.", "").Replace("+Request", "");
    };
});

builder.Services.AddEndpoints();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IEmailTokenService, EmailTokenService>();

builder.Services.AddScoped<IEmailService, SmtpEmailService>();

builder.Services.AddSharedTemplateService();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCookieAuthentication();

builder.Services.AddClientAppCorsPolicy();

builder
    .Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidationsMarker>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseCookieAuthentication();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseScalarApi();

    await AutomatedMigration.MigrateAsync<AppDbContext>(app.Services);

    app.UseMiddleware<PerformanceMiddleware>();
}

app.UseMiddleware<TransactionMiddleware>();

var appGroup = app.MapGroup("/api").AddFluentValidationAutoValidation();

app.MapEndpoints(appGroup);

app.Run();
