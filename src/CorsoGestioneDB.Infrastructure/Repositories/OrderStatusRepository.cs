using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class OrderStatusRepository : AbstractRepository, IOrderStatusRepository
{
    public OrderStatusRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<OrderStatus>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from OrderStatuses order by OrderStatusName";
        var orderStatuses = await db.QueryAsync<OrderStatus>(sql);

        return orderStatuses;
    }
}
