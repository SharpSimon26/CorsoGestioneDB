using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using CorsoGestioneDB.Application.Tests.Pipeline.Rules.OrderLine;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules.Order;

public class ReconstructOrderStatusRuleTests
{
    public record ReconstructOrderStatusRuleTestCase(
        string? OrderId,
        DateTime? OrderDate,
        string? OrderStatus,
        DateTime? DeliveryDate,
        string? ExpectedOrderStatus
    );

    public static TheoryData<ReconstructOrderStatusRuleTestCase> ReconstructOrderStatusData =
    [
        new(
            "ORD000358", new DateTime(2024, 12, 07, 03, 43, 44, DateTimeKind.Unspecified),
            "Unknown", new DateTime(2024, 12, 11, 0, 0, 0, DateTimeKind.Unspecified),
            "Consegnato"
        ),
        new(
            "ORD000420", new DateTime(2025, 06, 17, 10, 53, 15, DateTimeKind.Unspecified),
            null, new DateTime(2025, 06, 24, 0, 0, 0, DateTimeKind.Unspecified),
            "Consegnato"
        ),
        new(
            "ORD001256", new DateTime(2024, 05, 01, 23, 35, 18, DateTimeKind.Unspecified),
            null, null,
            "Unknown"
        )
    ];

    [Theory]
    [MemberData(nameof(ReconstructOrderStatusData))]
    public async Task Reconstruct_OrderStatus_From_Data(ReconstructOrderStatusRuleTestCase testCase)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.OrderID = testCase.OrderId;
        context.Data.Order.OrderDate = testCase.OrderDate;
        context.Data.Order.OrderStatus = testCase.OrderStatus;
        context.Data.Order.DeliveryDate = testCase.DeliveryDate;

        var reconstructOrderStatusRule = new ReconstructOrderStatusRule(NullLogger<ReconstructOrderStatusRule>.Instance);

        Assert.True(reconstructOrderStatusRule.CanApply(context));

        await reconstructOrderStatusRule.ApplyAsync(context);

        Assert.Equal(testCase.ExpectedOrderStatus, context.Data.Order.OrderStatus);
        Assert.Single(context.Messages);
    }
}