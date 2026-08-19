using CorsoGestioneDB.Infrastructure.Database;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public abstract class AbstractRepository
{
    protected readonly DbConnectionFactory connectionFactory;

    protected AbstractRepository(DbConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }
}
