using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface ISalesChannelRepository
{
    Task<IEnumerable<SalesChannel>> GetAllAsync();
    Task<SalesChannel?> GetByNameAsync(string channelName);
}
