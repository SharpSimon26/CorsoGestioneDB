using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules.OrderLine;

public class ReconstructRevenueRuleTests
{
    public static TheoryData<string?, int?, decimal?, int?, decimal?, decimal?, decimal?> ReconstructRevenueData = new()
    {
        {
            // OrderID    Qty  UnitPrice  DiscPct  ShipCost  Revenue    ExpRevenue
            "ORD006077",  3,   131.02m,   10,      0m,	     -50.00m,   353.75m
        },
        {
            "ORD000883",  2,   123.98m,   5,       8.90m,    -50.00m,   244.46m
        },
        {
            "ORD003016",  1,   28.89m,    10,      4.90m,	 -50.00m,   30.90m
        },
        {
            "ORD009999",  1,   416.22m,   5,       0m,       0m,        395.41m            
        }
    };

    [Theory]
    [MemberData(nameof(ReconstructRevenueData))]
    public async Task Reconstruct_Revenue_From_Data(string? orderId, int? quantity, decimal? unitPrice, int? discountPct, decimal? shippingCost, decimal? revenue, decimal? expectedRevenue)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = orderId;
        context.Data.OrderLine.Quantity = quantity;
        context.Data.OrderLine.UnitPrice = unitPrice;
        context.Data.OrderLine.DiscountPct = discountPct;
        context.Data.OrderLine.ShippingCost = shippingCost;
        context.Data.OrderLine.Revenue = revenue;

        var reconstructRevenueRule = new ReconstructRevenueRule(NullLogger<ReconstructRevenueRule>.Instance);

        Assert.True(reconstructRevenueRule.CanApply(context));

        await reconstructRevenueRule.ApplyAsync(context);

        Assert.Equal(expectedRevenue, context.Data.OrderLine.Revenue);
        Assert.Single(context.Modifications);
    }
}
