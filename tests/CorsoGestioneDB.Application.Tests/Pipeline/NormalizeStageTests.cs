using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
using CorsoGestioneDB.Application.Pipeline;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline;

public class NormalizeStageTests
{
    public static TheoryData<StagingOrder, string, string> NormalizeStageData = new()
    {
        {
            new StagingOrder { OrderID = "  ORD00001 ", Email = "   PIPPO@pluto.COM " }, 
            "ORD00001", "pippo@pluto.com"
        },
        {
            new StagingOrder { OrderID = "ORD00002    ", Email = "  TOPOLINO@minnie.net " }, 
            "ORD00002", "topolino@minnie.net"
        }
    };

    [Theory]
    [MemberData(nameof(NormalizeStageData))]
    public async Task NormalizeStage_Trims_Properties_And_Stuff(StagingOrder rawOrder, string expectedOrderId, string expectedEmail)
    {
        var context = new ImportContext(rawOrder);
        var contexts = new[] { context };
        var normalizeStage = new NormalizeStage(NullLogger<NormalizeStage>.Instance);
        
        await normalizeStage.ExecuteAsync(contexts);

        Assert.Equal(expectedOrderId, context.RawOrder.OrderID);
        Assert.Equal(expectedEmail, context.RawOrder.Email);        
        Assert.Equal(ImportRecordStatus.Pending, context.Status);
        Assert.Equal(2, context.Messages.Count);
    }
}