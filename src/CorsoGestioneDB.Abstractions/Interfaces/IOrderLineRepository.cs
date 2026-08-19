using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IOrderLineRepository
{
    Task<IEnumerable<OrderLine>> GetOrderLinesByOrderIdAsync(string orderId);
}
