using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class OrderRepository : AbstractRepository, IOrderRepository
{
    public OrderRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<Order?> GetOrderByOrderIdAsync(string orderId)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from Orders where OrderID = @orderId";
        var order = await db.QueryFirstOrDefaultAsync<Order>(sql, new { orderId });

        return order;
    }
}
