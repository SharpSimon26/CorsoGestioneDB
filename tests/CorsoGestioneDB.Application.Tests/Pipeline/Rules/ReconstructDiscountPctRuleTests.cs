using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules;

public class ReconstructDiscountPctRuleTests
{
    public static TheoryData<string?, int?, decimal?, int?, decimal?, decimal?, int?> ReconstructDiscountPctData = new()
    {
        {
            // OrderID    Qty  UnitPrice  DiscPct  ShipCost  Revenue   ExpDiscPct
            "ORD001536",  2,   304.55m,   110,     0m,       609.10m,  0
        },
        {
            "ORD009220",  1,   42.09m,    150,     12.90m,   54.99m,   0
        },
        {
            "ORD000711",  1,   97.63m,    -5,      12.90m,   100.77m,  10
        },
        {
            "ORD014257",  2,   89.34m,    -5,      8.90m,    169.71m,  10
        },
        {
            "ORD099999",  2,   500m,      -10,     8.90m,    18.90m,   99
        }
    };

    [Theory]
    [MemberData(nameof(ReconstructDiscountPctData))]
    public async Task Reconstruct_DiscountPct_From_Data(string? orderId, int? quantity, decimal? unitPrice, int? discountPct, decimal? shippingCost, decimal? revenue, int? expectedDiscountPct)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = orderId;
        context.Data.OrderLine.Quantity = quantity;
        context.Data.OrderLine.UnitPrice = unitPrice;
        context.Data.OrderLine.DiscountPct = discountPct;
        context.Data.OrderLine.ShippingCost = shippingCost;
        context.Data.OrderLine.Revenue = revenue;

        var reconstructDiscountPctRule = new ReconstructDiscountPctRule(NullLogger<ReconstructDiscountPctRule>.Instance);

        Assert.True(reconstructDiscountPctRule.CanApply(context));

        await reconstructDiscountPctRule.ApplyAsync(context);

        Assert.Equal(expectedDiscountPct, context.Data.OrderLine.DiscountPct);
        Assert.Single(context.Messages);
    }
}