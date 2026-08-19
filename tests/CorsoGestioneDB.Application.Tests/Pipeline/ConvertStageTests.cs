using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline;

public class ConvertStageTests
{
    public static TheoryData<StagingOrder, string, DateTime?, string, int, decimal, int?> ConvertStageData = new()
    {
        {
            new StagingOrder { OrderID = "ORD00001", OrderDate = "09/08/2026 14:30:00", Email = "pippo@pluto.com", Quantity = "5", 
                UnitPrice = "113.75", DiscountPct = "0" },
            "ORD00001", new DateTime(2026, 8, 9, 14, 30, 0, DateTimeKind.Unspecified), "pippo@pluto.com", 5, 113.75m, 0
        },
        {
            new StagingOrder { OrderID = "ORD00002", OrderDate = "", Email = "topolino@minnie.net", Quantity = "2", 
                UnitPrice = "11.43", DiscountPct = "dieci" },
            "ORD00002", null, "topolino@minnie.net", 2, 11.43m, null
        },
        {
            new StagingOrder { OrderID = "ORD013699", OrderDate = "2025-01-05 21:03:23", Email = "simone.rinaldi193@outlook.it", Quantity = "1", 
                UnitPrice = "453.35", DiscountPct = "-5" },
            "ORD013699", new DateTime(2025, 1, 5, 21, 03, 23, DateTimeKind.Unspecified), "simone.rinaldi193@outlook.it", 1, 453.35m, -5
        }
    };

    [Theory]
    [MemberData(nameof(ConvertStageData))]
    public async Task ConvertStage_Converts_Values_And_Populates_Data(StagingOrder rawOrder, string expectedOrderId, DateTime? expectedOrderDate,
                string expectedEmail, int expectedQuantity, decimal expectedUnitPrice, int? expectedDiscountPct)
    {
        var context = new ImportContext(rawOrder);
        var contexts = new[] { context };
        var convertStage = new ConvertStage(NullLogger<ConvertStage>.Instance);

        await convertStage.ExecuteAsync(contexts);

        Assert.Equal(expectedOrderId, context.Data.Order.OrderID);
        Assert.Equal(expectedOrderDate, context.Data.Order.OrderDate);
        Assert.Equal(expectedEmail, context.Data.Customer.Email);
        Assert.Equal(expectedQuantity, context.Data.OrderLine.Quantity);
        Assert.Equal(expectedUnitPrice, context.Data.OrderLine.UnitPrice);
        Assert.Equal(expectedDiscountPct, context.Data.OrderLine.DiscountPct);
    }
}