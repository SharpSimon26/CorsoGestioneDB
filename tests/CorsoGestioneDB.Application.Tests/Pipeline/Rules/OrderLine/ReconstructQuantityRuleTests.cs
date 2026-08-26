using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules.OrderLine;

public class ReconstructQuantityRuleTests
{
    public static TheoryData<string?, int?, decimal?, int?, decimal?, decimal?, int?> ReconstructQuantityData = new()
    {
        {
            // OrderID    Qty  UnitPrice  DiscPct  ShipCost  Revenue   ExpQty
            "ORD000490",  0,   375.62m,   25,      0m,       845.15m,  3
        },
        {
            "ORD006693",  0,   57.97m,    20,      12.90m,   59.28m,   1
        },
        {
            "ORD004560",  0,   881.51m,   5,       0m,       2512.30m, 3
        },
        {
            "ORD009999",  0,   100m,      5,       8.90m,    9413.90m, 99
        }
    };

    [Theory]
    [MemberData(nameof(ReconstructQuantityData))]
    public async Task Reconstruct_Quantity_From_Data(string? orderId, int? quantity, decimal? unitPrice, int? discountPct, decimal? shippingCost, decimal? revenue, int? expectedQuantity)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = orderId;
        context.Data.OrderLine.Quantity = quantity;
        context.Data.OrderLine.UnitPrice = unitPrice;
        context.Data.OrderLine.DiscountPct = discountPct;
        context.Data.OrderLine.ShippingCost = shippingCost;
        context.Data.OrderLine.Revenue = revenue;

        var reconstructQuantityRule = new ReconstructQuantityRule(NullLogger<ReconstructQuantityRule>.Instance);

        Assert.True(reconstructQuantityRule.CanApply(context));

        await reconstructQuantityRule.ApplyAsync(context);

        Assert.Equal(expectedQuantity, context.Data.OrderLine.Quantity);
        Assert.Single(context.Modifications);
    }
}
