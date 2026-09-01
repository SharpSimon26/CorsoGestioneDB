using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class PaymentMethodRepository : AbstractRepository, IPaymentMethodRepository
{
    public PaymentMethodRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public virtual async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from PaymentMethods order by PaymentMethodName";
        var paymentMethods = await db.QueryAsync<PaymentMethod>(sql);

        return paymentMethods;
    }

    public virtual async Task<PaymentMethod?> GetByNameAsync(string paymentMethodName)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from PaymentMethods where PaymentMethodName = @paymentMethodName";
        var paymentMethod = await db.QueryFirstOrDefaultAsync<PaymentMethod>(sql, new { paymentMethodName });

        return paymentMethod;
    }
}
