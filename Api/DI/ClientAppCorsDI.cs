

namespace Api.DI;


public static class ClientAppCorsDI
{
    extension(IServiceCollection services)
    {
        public void AddClientAppCorsPolicy()
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowClientApp",
                    policy =>
                    {
                        policy
                            .WithOrigins("http://localhost:4200", "https://localhost:4200") // Your frontend URL
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); // <--- CRITICAL
                    });
            });
        }
    }
}