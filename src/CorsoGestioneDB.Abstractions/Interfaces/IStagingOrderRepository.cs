using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IStagingOrderRepository
{
    Task<IEnumerable<StagingOrder>> GetAllAsync();
}
