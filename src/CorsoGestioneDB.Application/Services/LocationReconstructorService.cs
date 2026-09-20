using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Application.Services;

public class LocationReconstructorService : ILocationReconstructorService
{
    private readonly ConcurrentDictionary<string, StagingOrderLocationInfo> _cache;

    private readonly IStagingOrderRepository _stagingOrderRepository;

    public LocationReconstructorService(IStagingOrderRepository stagingOrderRepository)
    {
        _cache = new(StringComparer.OrdinalIgnoreCase);
        _stagingOrderRepository = stagingOrderRepository;
    }

    public async Task<StagingOrderLocationInfo?> ReconstructLocation(string cityName)
    {
        await EnsureCacheLoadedAsync();
        _cache.TryGetValue(cityName, out var locationInfo);

        return locationInfo;
    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var locationInfos = await _stagingOrderRepository.GetLocationInfoAsync();

        foreach (var item in locationInfos)
        {
            _cache.TryAdd(item.City, item);
        }
    }
}