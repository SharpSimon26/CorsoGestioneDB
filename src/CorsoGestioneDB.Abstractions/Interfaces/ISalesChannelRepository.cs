using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface ISalesChannelRepository
{
    Task<IEnumerable<SalesChannel>> GetAllAsync();
}
