using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules;

public class ReconstructQuantityRuleTests
{
    public static TheoryData<string?, decimal?, int?, decimal?, int?, decimal?, int?> ReconstructQuantityData = new()
    {
        {
            // OrderID   Revenue  Qty UnitPrice DiscPct ShipCost ExpQty
            "ORD000490", 845.15m, 0,  375.62m,  25,     0m,      3
        }
    };

    [Theory]
    [MemberData(nameof(ReconstructQuantityData))]
    public async Task Reconstruct_Quantity_From_Data(string? orderId, decimal? revenue, int? quantity, decimal? unitPrice, int? discountPct, decimal? shippingCost, int? expectedQuantity)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = orderId;
        context.Data.OrderLine.Revenue = revenue;
        context.Data.OrderLine.Quantity = quantity;
        context.Data.OrderLine.UnitPrice = unitPrice;
        context.Data.OrderLine.DiscountPct = discountPct;
        context.Data.OrderLine.ShippingCost = shippingCost;

        var reconstructQuantityRule = new ReconstructQuantityRule(NullLogger<ReconstructQuantityRule>.Instance);

        Assert.True(reconstructQuantityRule.CanApply(context));

        await reconstructQuantityRule.ApplyAsync(context);

        Assert.Equal(expectedQuantity, context.Data.OrderLine.Quantity);
    }
}
