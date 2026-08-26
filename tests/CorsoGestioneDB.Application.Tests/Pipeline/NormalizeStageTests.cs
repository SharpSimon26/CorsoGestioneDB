using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
using CorsoGestioneDB.Application.Pipeline;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline;

public class NormalizeStageTests
{
    public record NormalizeStageTestCase(
        string StagingOrderID,
        string StagingOrderEmail,
        string ExpectedOrderID,
        string ExpectedEmail
    );

    public static TheoryData<NormalizeStageTestCase> NormalizeStageData =
    [
        new (
            "  ORD00001 ", "   PIPPO@pluto.COM " , 
            "ORD00001", "pippo@pluto.com"
        ),
        new (
            "ORD00002    ", "  TOPOLINO@minnie.net " , 
            "ORD00002", "topolino@minnie.net"
        )
    ];

    [Theory]
    [MemberData(nameof(NormalizeStageData))]
    public async Task NormalizeStage_Trims_Properties_And_Stuff(NormalizeStageTestCase testCase)
    {
        var rawOrder = new StagingOrder
        {
            OrderID = testCase.StagingOrderID,
            Email = testCase.StagingOrderEmail
        };
        var context = new ImportContext(rawOrder);
        var contexts = new[] { context };
        var normalizeStage = new NormalizeStage(NullLogger<NormalizeStage>.Instance);
        
        await normalizeStage.ExecuteAsync(contexts);

        Assert.Equal(testCase.ExpectedOrderID, context.RawOrder.OrderID);
        Assert.Equal(testCase.ExpectedEmail, context.RawOrder.Email);        
        Assert.Equal(ImportRecordStatus.Pending, context.Status);
        Assert.Equal(2, context.Modifications.Count);
    }
}