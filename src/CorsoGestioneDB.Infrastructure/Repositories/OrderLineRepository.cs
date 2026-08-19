using System.Data;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class OrderLineRepository : AbstractRepository, IOrderLineRepository
{
    public OrderLineRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<OrderLine>> GetOrderLinesByOrderIdAsync(string orderId)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from OrderLines where OrderID = @orderId";
        var orderLines = await db.QueryAsync<OrderLine>(sql, new { orderId });

        return orderLines;
    }
}
