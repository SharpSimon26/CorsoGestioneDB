using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface ILocationInfoRepository
{
    Task<IEnumerable<LocationInfo>> GetAllAsync();
    Task<LocationInfo?> GetLocationInfoByCityNameAsync(string cityName);
}