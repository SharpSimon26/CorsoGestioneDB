using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Infrastructure.Database;
using System.Data;
using Dapper;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class StagingOrderRepository : AbstractRepository,  IStagingOrderRepository
{
    public StagingOrderRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<StagingOrder>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from StagingOrders order by OrderID";
        var stagingOrders = await db.QueryAsync<StagingOrder>(sql);

        return stagingOrders;
    }
}
