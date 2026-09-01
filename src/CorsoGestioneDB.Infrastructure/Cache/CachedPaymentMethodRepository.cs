using System.Collections.Concurrent;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Infrastructure.Cache;

public class CachedPaymentMethodRepository : ICachedPaymentMethodRepository
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly ConcurrentDictionary<string, PaymentMethod> _cache;

    public CachedPaymentMethodRepository(IPaymentMethodRepository paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _cache = new(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        await EnsureCacheLoadedAsync();

        return _cache.Values.ToList();
    }

    public async Task<PaymentMethod?> GetByNameAsync(string paymentMethodName)
    {
        await EnsureCacheLoadedAsync();
        _cache.TryGetValue(paymentMethodName, out var paymentMethod);
        
        return paymentMethod;
    }

    private async Task EnsureCacheLoadedAsync()
    {
        if (!_cache.IsEmpty)
        {
            return;
        }

        var orderStatuses = await _paymentMethodRepository.GetAllAsync();

        foreach (var item in orderStatuses)
        {
            _cache.TryAdd(item.PaymentMethodName, item);
        }
    }
}