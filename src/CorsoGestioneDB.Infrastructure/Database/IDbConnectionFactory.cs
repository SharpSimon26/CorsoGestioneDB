using System.Data;

namespace CorsoGestioneDB.Infrastructure.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}