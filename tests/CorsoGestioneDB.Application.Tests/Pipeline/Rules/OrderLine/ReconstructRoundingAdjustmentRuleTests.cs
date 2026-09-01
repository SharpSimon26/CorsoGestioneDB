using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules.OrderLine;

public class ReconstructRoundingAdjustmentRuleTests
{
    public record ReconstructRoundingAdjustmentRuleTestCase(
        string? OrderId,
        int? Quantity,
        decimal? UnitPrice,
        int? DiscountPct,
        decimal? ShippingCost,
        decimal? Revenue,
        decimal? ExpectedAdjustment,
        bool IsRejected);

    public static TheoryData<ReconstructRoundingAdjustmentRuleTestCase> ReconstructRoundingAdjustmentRuleData =
    [
        // OrderID        Qty  UnitPrice  DiscPct  ShipCost  Revenue    ExpAdjustment  IsRejected
        new("ORD000784",  1,   1234.50m,  20,      4.90m,    153.44m,   839.06m,       true),
        new("ORD000098",  3,   125.78m,   25,      0m,       283.00m,   0.01m,         false)
    ];

    [Theory]
    [MemberData(nameof(ReconstructRoundingAdjustmentRuleData))]
    public async Task Rounding_Revenue_Adjusts_Or_Rejects(ReconstructRoundingAdjustmentRuleTestCase testCase)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = testCase.OrderId;
        context.Data.OrderLine.Quantity = testCase.Quantity;
        context.Data.OrderLine.UnitPrice = testCase.UnitPrice;
        context.Data.OrderLine.DiscountPct = testCase.DiscountPct;
        context.Data.OrderLine.ShippingCost = testCase.ShippingCost;
        context.Data.OrderLine.Revenue = testCase.Revenue;

        var roundingAdjustmentRule = new ReconstructRoundingAdjustmentRule();

        Assert.True(roundingAdjustmentRule.CanApply(context));

        await roundingAdjustmentRule.ApplyAsync(context);

        Assert.Equal(testCase.ExpectedAdjustment, context.Data.OrderLine.RoundingAdj);
        Assert.Equal(testCase.IsRejected, context.IsRejected());
        Assert.Single(context.Modifications);
    }
}