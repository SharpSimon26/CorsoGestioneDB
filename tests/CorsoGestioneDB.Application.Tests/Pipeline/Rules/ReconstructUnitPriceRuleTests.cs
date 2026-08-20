using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules;

public class ReconstructUnitPriceRuleTests
{
    public static TheoryData<string?, int?, decimal?, int?, decimal?, decimal?, decimal?> ReconstructUnitPriceData = new()
    {
        {
            // OrderID    Qty  UnitPrice  DiscPct  ShipCost  Revenue    ExpUnitPrice
            "ORD003193",  1,   0m,        5,       0m,       395.41m,   416.22m
        },
        {
            "ORD001296",  1,   0m,        20,      6.90m,    164.38m,   196.85m
        },
        {
            "ORD004631",  5,   0m,        5,       0m,       2298.00m,  483.79m
        },
        {
            "ORD006718",  1,   0m,        15,      12.90m,   89.25m,    89.82m
        },
        {
            "ORD009999",  15,  0m,        25,      8.90m,    5840.34m,  518.35m
        }
    };

    [Theory]
    [MemberData(nameof(ReconstructUnitPriceData))]
    public async Task Reconstruct_UnitPrice_From_Data(string? orderId, int? quantity, decimal? unitPrice, int? discountPct, decimal? shippingCost, decimal? revenue, decimal? expectedUnitPrice)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = orderId;
        context.Data.OrderLine.Quantity = quantity;
        context.Data.OrderLine.UnitPrice = unitPrice;
        context.Data.OrderLine.DiscountPct = discountPct;
        context.Data.OrderLine.ShippingCost = shippingCost;
        context.Data.OrderLine.Revenue = revenue;

        var reconstructUnitPriceRule = new ReconstructUnitPriceRule(NullLogger<ReconstructUnitPriceRule>.Instance);

        Assert.True(reconstructUnitPriceRule.CanApply(context));

        await reconstructUnitPriceRule.ApplyAsync(context);

        Assert.Equal(expectedUnitPrice, context.Data.OrderLine.UnitPrice);
        Assert.Single(context.Messages);
    }   
}
