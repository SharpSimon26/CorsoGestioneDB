using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IOrderStatusRepository
{
    Task<IEnumerable<OrderStatus>> GetAllAsync();
}
