namespace Api.DI;

public static class OpenApiExtensions
{
    public static IServiceCollection AddCustomOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.CreateSchemaReferenceId = (json) =>
            {
                if (json.Type.FullName == null)
                    return json.Type.Name;

                return json.Type.FullName.Replace("Api.Features.", "").Replace("+Request", "");
            };
        });

        return services;
    }
}
