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
        var sql = "select * from SalesChannels order by ChannelName";
        var salesChannels = await db.QueryAsync<SalesChannel>(sql);

        return salesChannels;
    }

    public virtual async Task<SalesChannel?> GetByNameAsync(string channelName)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from SalesChannels where ChannelName = @channelName";
        var salesChannel = await db.QueryFirstOrDefaultAsync<SalesChannel>(sql, new { channelName });

        return salesChannel;
    }
}