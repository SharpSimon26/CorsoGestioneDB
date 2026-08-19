using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Abstractions.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByIdAsync(int customerId);
}
