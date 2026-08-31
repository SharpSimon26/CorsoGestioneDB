using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;

namespace CorsoGestioneDB.Infrastructure.Cache;

public class CachedCategoryRepository : CategoryRepository, ICachedCategoryRepository
{
    private readonly ConcurrentDictionary<string, Category> _cache;

    public CachedCategoryRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
        _cache = new(StringComparer.OrdinalIgnoreCase);
    }

    public override async Task<IEnumerable<Category>> GetAllAsync()
    {
        if (!_cache.Any())
        {
            await EnsureCacheLoadedAsync();
        }

        return _cache.Values.ToList();
    }

    public override async Task<Category?> GetByNameAsync(string categoryName)
    {
        if (!_cache.Any())
        {
            await EnsureCacheLoadedAsync();
        }

        _cache.TryGetValue(categoryName, out var category);
        
        return category;

    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var categories = await base.GetAllAsync();

        foreach (var item in categories)
        {
            _cache.TryAdd(item.CategoryName, item);
        }
    }
}
