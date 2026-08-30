using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Application.Services;

public class ProductCodeResolverService : IProductCodeResolverService
{
    private readonly ConcurrentDictionary<string, StagingOrderProductInfo> _cache;
    private readonly IStagingOrderRepository _stagingOrderRepository;

    public ProductCodeResolverService(IStagingOrderRepository stagingOrderRepository)
    {
        _cache = new(StringComparer.OrdinalIgnoreCase);
        _stagingOrderRepository = stagingOrderRepository;
    }

    public async Task<StagingOrderProductInfo?> ResolveProductCode(string productName)
    {
        if (!_cache.Any())
        {
            await EnsureCacheLoadedAsync();
        }

        _cache.TryGetValue(productName, out var productInfo);

        return productInfo;
    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var productInfos = await _stagingOrderRepository.GetProductInfoAsync();

        foreach (var item in productInfos)
        {
            _cache.TryAdd(item.ProductName, item);
        }
    }
}
