using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IOrderStatusRepository
{
    Task<OrderStatus?> GetByNameAsync(string orderStatusName);
    Task<IEnumerable<OrderStatus>> GetAllAsync();
}
