using System.Data;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class CustomerRepository : AbstractRepository, ICustomerRepository
{
    public CustomerRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from Customers where CustomerID = @customerId";
        var customer = await db.QueryFirstOrDefaultAsync<Customer>(sql, new { customerId });

        return customer;
    }
}
