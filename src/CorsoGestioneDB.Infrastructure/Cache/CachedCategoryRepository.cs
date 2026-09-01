using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Infrastructure.Cache;

public class CachedCategoryRepository : ICachedCategoryRepository
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ConcurrentDictionary<string, Category> _cache;

    public CachedCategoryRepository(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _cache = new(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        await EnsureCacheLoadedAsync();

        return _cache.Values.ToList();
    }

    public async Task<Category?> GetByNameAsync(string categoryName)
    {
        await EnsureCacheLoadedAsync();
        _cache.TryGetValue(categoryName, out var category);
        
        return category;
    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var categories = await _categoryRepository.GetAllAsync();

        foreach (var item in categories)
        {
            _cache.TryAdd(item.CategoryName, item);
        }
    }
}
