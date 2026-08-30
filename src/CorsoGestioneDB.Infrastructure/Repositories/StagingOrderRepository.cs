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
}
