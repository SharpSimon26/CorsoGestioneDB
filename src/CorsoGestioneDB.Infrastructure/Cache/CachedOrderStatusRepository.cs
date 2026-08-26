using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;

namespace CorsoGestioneDB.Infrastructure.Cache;

public class CachedOrderStatusRepository : OrderStatusRepository, ICachedOrderStatusRepository
{
    private readonly ConcurrentDictionary<string, OrderStatus> _cache;

    public CachedOrderStatusRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
        _cache = new(StringComparer.OrdinalIgnoreCase);
    }

    public override async Task<IEnumerable<OrderStatus>> GetAllAsync()
    {
        if (!_cache.Any())
        {
            await EnsureCacheLoadedAsync();
        }

        return _cache.Values.ToList();
    }

    public override async Task<OrderStatus?> GetByNameAsync(string orderStatusName)
    {
        if (!_cache.Any())
        {
            await EnsureCacheLoadedAsync();
        }

        _cache.TryGetValue(orderStatusName, out var orderStatus);
        
        return orderStatus;

    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var orderStatuses = await base.GetAllAsync();

        foreach (var item in orderStatuses)
        {
            _cache.TryAdd(item.OrderStatusName, item);
        }
    }
}