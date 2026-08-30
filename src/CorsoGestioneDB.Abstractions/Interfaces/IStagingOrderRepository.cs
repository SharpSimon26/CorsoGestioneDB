using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IStagingOrderRepository
{
    Task<IEnumerable<StagingOrder>> GetAllAsync();
    Task<IEnumerable<StagingOrderProductInfo>> GetProductInfoAsync();
}
