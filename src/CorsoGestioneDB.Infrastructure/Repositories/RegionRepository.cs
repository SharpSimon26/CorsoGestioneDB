using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class RegionRepository : AbstractRepository, IRegionRepository
{
    public RegionRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {        
    }

    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from Regions order by RegionName";
        var regions = await db.QueryAsync<Region>(sql);

        return regions;
    }
}
