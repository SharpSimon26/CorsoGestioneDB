using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules;

public class ReconstructUnitPriceRuleTests
{
    public static TheoryData<string?, decimal?, int?, decimal?, int?, decimal?, decimal?> ReconstructUnitPriceData = new()
    {
        {
            // OrderID   Revenue  Qty UnitPrice DiscPct ShipCost ExpUnitPrice
            "ORD003193", 395.41m, 1,  0m,       5,       0m,     416.22m
        }
    };

    [Theory]
    [MemberData(nameof(ReconstructUnitPriceData))]
    public async Task Reconstruct_UnitPrice_From_Data(string? orderId, decimal? revenue, int? quantity, decimal? unitPrice, int? discountPct, decimal? shippingCost, decimal? expectedUnitPrice)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = orderId;
        context.Data.OrderLine.Revenue = revenue;
        context.Data.OrderLine.Quantity = quantity;
        context.Data.OrderLine.UnitPrice = unitPrice;
        context.Data.OrderLine.DiscountPct = discountPct;
        context.Data.OrderLine.ShippingCost = shippingCost;

        var reconstructUnitPriceRule = new ReconstructUnitPriceRule(NullLogger<ReconstructUnitPriceRule>.Instance);

        Assert.True(reconstructUnitPriceRule.CanApply(context));

        await reconstructUnitPriceRule.ApplyAsync(context);

        Assert.Equal(expectedUnitPrice, context.Data.OrderLine.UnitPrice);
    }   
}
