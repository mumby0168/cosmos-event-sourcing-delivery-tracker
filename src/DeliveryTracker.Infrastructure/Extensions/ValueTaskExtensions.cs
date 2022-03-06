namespace DeliveryTracker.Infrastructure.Extensions;

public static class ValueTaskExtensions
{
    public static async ValueTask<List<T>> ToListAsync<T>(this ValueTask<IEnumerable<T>> task)
    {
        var enumerable = await task;
        return enumerable.ToList();
    }
}