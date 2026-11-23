using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync();
    Task<PaymentMethod?> GetByNameAsync(string paymentMethodName);
    Task<PaymentMethod?> GetBestMatchByNameAsync(string paymentMethodName, int minimumSimilarity = 80, int minimumGap = 10);
}
