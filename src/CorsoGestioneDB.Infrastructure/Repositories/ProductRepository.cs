using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class ProductRepository : AbstractRepository, IProductRepository
{
    public ProductRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<Product?> GetProductByProductCodeAsync(string productCode)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from Products where ProductCode = @productCode";
        var product = await db.QueryFirstOrDefaultAsync<Product>(sql, new { productCode });

        return product;
    }
}
