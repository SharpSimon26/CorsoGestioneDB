using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetProductByProductCodeAsync(string productCode);
}
