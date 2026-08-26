using CorsoGestioneDB.Infrastructure.Database;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public abstract class AbstractRepository
{
    protected readonly IDbConnectionFactory connectionFactory;

    protected AbstractRepository(IDbConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }
}
