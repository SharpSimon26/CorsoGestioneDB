using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Order;

public class ReconstructOrderDateRuleTests
{
    public record ReconstructOrderDateTestCase(
        string? OrderId,
        DateTime? OrderDate,
        DateTime? DeliveryDate,
        bool ExpectedApply,
        DateTime ExpectedOrderDate
    );

    public static TheoryData<ReconstructOrderDateTestCase> ReconstructOrderDateData =
    [
        new(
            "ORD001146", null, new DateTime(2024, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), 
            true, new DateTime(2024, 11, 06, 0, 0, 0, DateTimeKind.Unspecified)
        )
    ];

    [Theory]
    [MemberData(nameof(ReconstructOrderDateData))]
    public async Task Reconstruct_OrderDate_From_DeliveryDate_If_Null(ReconstructOrderDateTestCase testCase)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = testCase.OrderId;
        context.Data.Order.OrderDate = testCase.OrderDate;
        context.Data.Order.DeliveryDate = testCase.DeliveryDate;

        var reconstructOrderDateRule = new ReconstructOrderDateRule();

        var canApplyToContext = reconstructOrderDateRule.CanApply(context);
        Assert.Equal(testCase.ExpectedApply, canApplyToContext);

        await reconstructOrderDateRule.ApplyAsync(context);

        Assert.Equal(testCase.ExpectedOrderDate, context.Data.Order.OrderDate);
        Assert.Single(context.Modifications);
    }
}