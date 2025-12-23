using Infrastructure;

namespace Api.Middleware;

public class TransactionMiddleware(RequestDelegate next, ILogger<TransactionMiddleware> logger)
{
    private readonly ILogger<TransactionMiddleware> _logger = logger;

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, SarhneDbContext dbContext)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await _next(context);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            _logger.LogError("Transaction failed");
            throw;
        }
    }
}
