using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class AutomatedMigration
{
    public static async Task MigrateAsync<TDbContext>(IServiceProvider services)
        where TDbContext : DbContext
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}
