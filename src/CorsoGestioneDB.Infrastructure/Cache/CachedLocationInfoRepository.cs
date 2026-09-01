using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Infrastructure.Cache;

public class CachedLocationInfoRepository : ICachedLocationInfoRepository
{
    private readonly ILocationInfoRepository _locationInfoRepository;
    private readonly ConcurrentDictionary<string, LocationInfo> _cache;

    public CachedLocationInfoRepository(ILocationInfoRepository locationInfoRepository)
    {
        _locationInfoRepository = locationInfoRepository;
        _cache = new(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<LocationInfo>> GetAllAsync()
    {
        await EnsureCacheLoadedAsync();

        return _cache.Values.ToList();
    }

    public async Task<LocationInfo?> GetLocationInfoByCityNameAsync(string cityName)
    {
        await EnsureCacheLoadedAsync();
        _cache.TryGetValue(cityName, out LocationInfo? locationInfo);
        
        return locationInfo;
    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var locations = await _locationInfoRepository.GetAllAsync();

        foreach (var item in locations)
        {
            _cache.TryAdd(item.CityName, item);
        }
    }
}
