using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAllAsync();
}
