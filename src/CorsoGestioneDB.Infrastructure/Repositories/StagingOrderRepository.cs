using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Infrastructure.Database;
using System.Data;
using Dapper;
using CorsoGestioneDB.Domain.Models;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class StagingOrderRepository : AbstractRepository,  IStagingOrderRepository
{
    public StagingOrderRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<StagingOrder>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from StagingOrders order by OrderID";
        var stagingOrders = await db.QueryAsync<StagingOrder>(sql);

        return stagingOrders;
    }

    public async Task<IEnumerable<StagingOrderProductInfo>> GetProductInfoAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = @"
            with ProductUpper as (
                select 
                    UPPER(TRIM(ProductCode)) as ProductCodeUp, 
                    TRIM(ProductName) as ProductNameTrim
                from StagingOrders
            ),
            ProductNumOrders as (
                select 
                    ProductCodeUp as ProductCode,
                    ProductNameTrim as ProductName,
                    count(*) as NumOrders
                from ProductUpper
                group by ProductCodeUp, ProductNameTrim
                having ProductCodeUp is not null
            ),
            ProductCodeUsage as (
                select 
                    ProductCode,
                    ProductName,
                    NumOrders,
                    CAST(100.0 * NumOrders / SUM(NumOrders) OVER (PARTITION BY ProductName) AS DECIMAL(6,3)) as UsagePercentage
                from ProductNumOrders
            ),
            ProductRowNum as (
                select
                    ProductCode,
                    ProductName,
                    NumOrders,
                    UsagePercentage,
                    ROW_NUMBER() OVER (PARTITION BY ProductName ORDER BY UsagePercentage DESC) as RowNum
                from ProductCodeUsage
            )
            select 
                ProductCode,
                ProductName,
                NumOrders,
                UsagePercentage
            from ProductRowNum
            where RowNum = 1
            order by ProductName";

        var productInfos = await db.QueryAsync<StagingOrderProductInfo>(sql);

        return productInfos;
    }

    public async Task<IEnumerable<StagingOrderLocationInfo>> GetLocationInfoAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = @"
            with CustomerLocations as (
                select 
                    UPPER(LEFT(City, 1))+SUBSTRING(City, 2) City,
                    UPPER(LEFT(Province, 1))+SUBSTRING(Province, 2) Province,
                    UPPER(LEFT(Region, 1))+SUBSTRING(Region, 2) Region,
                    count(*) NumOrders
                from StagingOrders
                where City is not null 
                    and Province is not null 
                    and Region is not null
                    and Region != 'N/D'
                group by City, Province, Region
            ),
            CustomerLocationUsage as (
                select 
                    City,
                    Province,
                    Region,
                    NumOrders,
                    CAST(100.0 * NumOrders / SUM(NumOrders) OVER (PARTITION BY City) AS DECIMAL(6,3)) as UsagePercentage
                from CustomerLocations
            ),
            CustomerLocationRowNum as (
                select
                    City,
                    Province,
                    Region,
                    NumOrders,
                    UsagePercentage,
                    ROW_NUMBER() OVER (PARTITION BY City ORDER BY UsagePercentage DESC) as RowNumber
                from CustomerLocationUsage
            )
            select 
                City,
                Province,
                Region,
                NumOrders,
                UsagePercentage
            from CustomerLocationRowNum
            where RowNumber = 1
            order by City";
        
        var locationInfos = await db.QueryAsync<StagingOrderLocationInfo>(sql);

        return locationInfos;
    }
}
