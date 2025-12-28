namespace Infrastructure.Extensions;

public static class BaseEntityExtensions
{
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}
