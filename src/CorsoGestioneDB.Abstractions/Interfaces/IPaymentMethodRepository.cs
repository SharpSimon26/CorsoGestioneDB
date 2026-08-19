using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync();
}
