using System.Data;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Models;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class LocationInfoRepository : AbstractRepository, ILocationInfoRepository
{
    public LocationInfoRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<LocationInfo>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = @"select ci.CityID, ci.CityName, pr.ProvinceID, pr.ProvinceName, re.RegionID, re.RegionName 
                  from Cities ci 
                  inner join Provinces pr on pr.ProvinceID = ci.ProvinceID 
                  inner join Regions re on re.RegionID = pr.RegionID";
        var locationInfos = await db.QueryAsync<LocationInfo>(sql);

        return locationInfos;
    }

    public async Task<LocationInfo?> GetLocationInfoByCityNameAsync(string cityName)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = @"select ci.CityID, ci.CityName, pr.ProvinceID, pr.ProvinceName, re.RegionID, re.RegionName 
                  from Cities ci 
                  inner join Provinces pr on pr.ProvinceID = ci.ProvinceID 
                  inner join Regions re on re.RegionID = pr.RegionID
                  where ci.CityName = @cityName";
        var locationInfo = await db.QueryFirstOrDefaultAsync(sql, new { cityName });
        
        return locationInfo;
    }
}