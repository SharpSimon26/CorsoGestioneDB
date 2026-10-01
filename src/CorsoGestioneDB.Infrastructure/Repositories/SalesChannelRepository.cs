using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class SalesChannelRepository : AbstractRepository, ISalesChannelRepository
{
    public SalesChannelRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {        
    }

    public virtual async Task<IEnumerable<SalesChannel>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select SalesChannelID, SalesChannelName from SalesChannels order by SalesChannelName";
        var salesChannels = await db.QueryAsync<SalesChannel>(sql);

        return salesChannels;
    }

    public virtual async Task<SalesChannel?> GetByNameAsync(string salesChannelName)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select SalesChannelID, SalesChannelName from SalesChannels where SalesChannelName = @salesChannelName";
        var salesChannel = await db.QueryFirstOrDefaultAsync<SalesChannel>(sql, new { salesChannelName });

        return salesChannel;
    }
}