using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Infrastructure.Cache;


public class CachedSalesChannelRepository : ICachedSalesChannelRepository
{
    private readonly ISalesChannelRepository _salesChannelRepository;

    private readonly ConcurrentDictionary<string, SalesChannel> _cache;

    public CachedSalesChannelRepository(ISalesChannelRepository salesChannelRepository)
    {
        _salesChannelRepository = salesChannelRepository;
        _cache = new(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<SalesChannel>> GetAllAsync()
    {
        await EnsureCacheLoadedAsync();

        return _cache.Values.ToList();
    }

    public async Task<SalesChannel?> GetByNameAsync(string salesChannelName)
    {
        await EnsureCacheLoadedAsync();

        _cache.TryGetValue(salesChannelName, out var salesChannel);

        return salesChannel;
    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var salesChannels = await _salesChannelRepository.GetAllAsync();

        foreach (var item in salesChannels)
        {
            _cache.TryAdd(item.SalesChannelName, item);
        }
    }
}