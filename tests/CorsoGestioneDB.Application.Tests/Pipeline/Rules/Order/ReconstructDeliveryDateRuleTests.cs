using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Order;

public class ReconstructDeliveryDateRuleTests
{
    public record ReconstructDeliveryDateTestCase(
        string? OrderId,
        DateTime? OrderDate,
        DateTime? DeliveryDate,
        bool ExpectedApply,
        DateTime ExpectedDeliveryDate
    );

    public static TheoryData<ReconstructDeliveryDateTestCase> ReconstructDeliveryDateData =
    [
        new(
            "ORD000128", 
            new DateTime(2025, 06, 26, 20, 10, 32, DateTimeKind.Unspecified), 
            new DateTime(2023, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            true,
            new DateTime(2025, 06, 30, 20, 10, 32, DateTimeKind.Unspecified)
        ),
        new(
            "ORD000165",
            new DateTime(2025, 09, 10, 23, 12, 33, DateTimeKind.Unspecified),
            new DateTime(2023, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            true,
            new DateTime(2025, 09, 14, 23, 12, 33, DateTimeKind.Unspecified)
        )
    ];

    [Theory]
    [MemberData(nameof(ReconstructDeliveryDateData))]
    public async Task Reconstruct_DeliveryDate_From_OrderDate(ReconstructDeliveryDateTestCase testCase)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = testCase.OrderId;
        context.Data.Order.OrderDate = testCase.OrderDate;
        context.Data.Order.DeliveryDate = testCase.DeliveryDate;

        var reconstructDeliveryDateRule = new ReconstructDeliveryDateRule();

        var canApplyToContext = reconstructDeliveryDateRule.CanApply(context);
        Assert.Equal(testCase.ExpectedApply, canApplyToContext);

        await reconstructDeliveryDateRule.ApplyAsync(context);
        Assert.Equal(testCase.ExpectedDeliveryDate, context.Data.Order.DeliveryDate);
        Assert.Single(context.Modifications);
    }
}