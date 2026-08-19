using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class PaymentMethodRepository : AbstractRepository, IPaymentMethodRepository
{
    public PaymentMethodRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from PaymentMethods order by PaymentMethodName";
        var paymentMethods = await db.QueryAsync<PaymentMethod>(sql);

        return paymentMethods;
    }
}
