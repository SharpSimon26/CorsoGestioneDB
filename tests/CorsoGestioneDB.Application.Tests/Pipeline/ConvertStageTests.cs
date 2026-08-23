using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline;

public class ConvertStageTests
{
    public record ConvertStageTestCase(
        StagingOrder RawOrder,
        string ExpectedOrderId,
        DateTime? ExpectedOrderDate,
        string ExpectedEmail,
        int ExpectedQuantity,
        decimal ExpectedUnitPrice,
        int? ExpectedDiscountPct
    );

    public static TheoryData<ConvertStageTestCase> ConvertStageData =
    [
        new (
            new StagingOrder { OrderID = "ORD00001", OrderDate = "09/08/2026 14:30:00", Email = "pippo@pluto.com", Quantity = "5", 
                UnitPrice = "113.75", DiscountPct = "0" },
            "ORD00001", new DateTime(2026, 8, 9, 14, 30, 0, DateTimeKind.Unspecified), "pippo@pluto.com", 5, 113.75m, 0
        ),
        new (
            new StagingOrder { OrderID = "ORD00002", OrderDate = "", Email = "topolino@minnie.net", Quantity = "2", 
                UnitPrice = "11.43", DiscountPct = "dieci" },
            "ORD00002", null, "topolino@minnie.net", 2, 11.43m, null
        ),
        new (
            new StagingOrder { OrderID = "ORD013699", OrderDate = "2025-01-05 21:03:23", Email = "simone.rinaldi193@outlook.it", Quantity = "1", 
                UnitPrice = "453.35", DiscountPct = "-5" },
            "ORD013699", new DateTime(2025, 1, 5, 21, 03, 23, DateTimeKind.Unspecified), "simone.rinaldi193@outlook.it", 1, 453.35m, -5
        )
    ];

    [Theory]
    [MemberData(nameof(ConvertStageData))]
    public async Task ConvertStage_Converts_Values_And_Populates_Data(ConvertStageTestCase testCase)
    {
        var context = new ImportContext(testCase.RawOrder);
        var contexts = new[] { context };
        var convertStage = new ConvertStage(NullLogger<ConvertStage>.Instance);

        await convertStage.ExecuteAsync(contexts);

        Assert.Equal(testCase.ExpectedOrderId, context.Data.Order.OrderID);
        Assert.Equal(testCase.ExpectedOrderDate, context.Data.Order.OrderDate);
        Assert.Equal(testCase.ExpectedEmail, context.Data.Customer.Email);
        Assert.Equal(testCase.ExpectedQuantity, context.Data.OrderLine.Quantity);
        Assert.Equal(testCase.ExpectedUnitPrice, context.Data.OrderLine.UnitPrice);
        Assert.Equal(testCase.ExpectedDiscountPct, context.Data.OrderLine.DiscountPct);
    }
}