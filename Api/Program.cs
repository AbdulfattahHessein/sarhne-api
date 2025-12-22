using System.Reflection;
using Api.DI;
using Api.Extensions;
using Api.Interfaces;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEndpoints();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<SarhneDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCookieAuthentication();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200") // Your frontend URL
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // <--- CRITICAL
        }
    );
});

builder
    .Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidationsMarker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(config =>
    {
        app.MapGet("/", () => Results.Redirect("/scalar"))
            .ExcludeFromDescription()
            .ExcludeFromApiReference();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookieAuthentication();

var appGroup = app.MapGroup("/api").AddFluentValidationAutoValidation();

app.MapEndpoints(appGroup);

app.Run();
