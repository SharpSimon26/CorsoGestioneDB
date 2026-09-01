using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Infrastructure.Cache;

public class CachedOrderStatusRepository : ICachedOrderStatusRepository
{
    private readonly IOrderStatusRepository _orderStatusRepository;
    private readonly ConcurrentDictionary<string, OrderStatus> _cache;

    public CachedOrderStatusRepository(IOrderStatusRepository orderStatusRepository)
    {
        _orderStatusRepository = orderStatusRepository;
        _cache = new(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<OrderStatus>> GetAllAsync()
    {
        await EnsureCacheLoadedAsync();

        return _cache.Values.ToList();
    }

    public async Task<OrderStatus?> GetByNameAsync(string orderStatusName)
    {
        await EnsureCacheLoadedAsync();
        _cache.TryGetValue(orderStatusName, out var orderStatus);
        
        return orderStatus;
    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var orderStatuses = await _orderStatusRepository.GetAllAsync();

        foreach (var item in orderStatuses)
        {
            _cache.TryAdd(item.OrderStatusName, item);
        }
    }
}