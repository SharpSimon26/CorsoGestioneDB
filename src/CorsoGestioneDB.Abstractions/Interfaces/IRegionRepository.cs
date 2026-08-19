using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetAllAsync();
}
