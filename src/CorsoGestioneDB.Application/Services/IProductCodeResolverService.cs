using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Application.Services;

public interface IProductCodeResolverService
{
    Task<StagingOrderProductInfo?> ResolveProductCode(string productName);
}