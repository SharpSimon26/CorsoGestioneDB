using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Application.Services;

public interface ILocationReconstructorService
{
    Task<StagingOrderLocationInfo?> ReconstructLocation(string cityName);
}