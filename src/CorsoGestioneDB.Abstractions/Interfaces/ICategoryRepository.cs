using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
}
