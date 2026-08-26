using System.Data;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class CustomerRepository : AbstractRepository, ICustomerRepository
{
    public CustomerRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public virtual async Task<Customer?> GetByIdAsync(int customerId)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from Customers where CustomerID = @customerId";
        var customer = await db.QueryFirstOrDefaultAsync<Customer>(sql, new { customerId });

        return customer;
    }
}
