using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Infrastructure.Cache;

public class CachedLocationInfoRepository : ICachedLocationInfoRepository
{
    private readonly ILocationInfoRepository _locationInfoRepository;
    private readonly Dictionary<string, LocationInfo> _cache;
    private readonly ILogger<CachedLocationInfoRepository> _logger;

    public CachedLocationInfoRepository(ILocationInfoRepository locationInfoRepository, ILogger<CachedLocationInfoRepository> logger)
    {
        _locationInfoRepository = locationInfoRepository;
        _cache = new(StringComparer.OrdinalIgnoreCase);
        _logger = logger;
    }

    public async Task<IEnumerable<LocationInfo>> GetAllAsync()
    {
        if (_cache.Any())
        {
            return _cache.Values.ToList();
        }

        var locations = await _locationInfoRepository.GetAllAsync();

        foreach (var location in locations)
        {
            _cache.TryAdd(location.CityName, location);
        }
        
        return locations;
    }

    public async Task<LocationInfo?> GetLocationInfoByCityNameAsync(string cityName)
    {
        if (_cache.TryGetValue(cityName, out LocationInfo? locationInfo))
        {
            return locationInfo;
        }

        var location = await _locationInfoRepository.GetLocationInfoByCityNameAsync(cityName);

        if (location != null)
        {
            _cache.TryAdd(location.CityName, location);
        }

        return location;
    }
}
