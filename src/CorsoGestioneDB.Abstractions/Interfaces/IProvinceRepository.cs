using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IProvinceRepository
{
    Task<IEnumerable<Province>> GetAllAsync();
}
