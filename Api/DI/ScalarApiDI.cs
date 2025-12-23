using Scalar.AspNetCore;

namespace Api.DI;

public static class ScalarApiDI
{
    extension(WebApplication app)
    {
        public void UseScalarApi()
        {
            app.MapOpenApi();

            app.MapScalarApiReference(config =>
            {
                app.MapGet("/", () => Results.Redirect("/scalar"))
                    .ExcludeFromDescription()
                    .ExcludeFromApiReference();

                config.Layout = ScalarLayout.Classic;
                config.Title = "Sarhne API";
            });
        }
    }
}
