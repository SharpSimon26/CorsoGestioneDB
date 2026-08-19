using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
using CorsoGestioneDB.Application.Pipeline;
using CorsoGestioneDB.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace CorsoGestioneDB.Application.Tests.Pipeline;

public class DuplicateStageTests
{
    public static TheoryData<StagingOrder, StagingOrder> DuplicateStageData = new()
    {
        {
            new StagingOrder { OrderID = "ORD00001", Email = "pippo@pluto.com" },
            new StagingOrder { OrderID = "ORD00001", Email = "pippo@pluto.com" }
        }
    };

    [Theory]
    [MemberData(nameof(DuplicateStageData))]
    public async Task DuplicateStage_Marks_Rows_After_First_As_Duplicate(StagingOrder order1, StagingOrder order2)
    {
        var context1 = new ImportContext(order1);
        var context2 = new ImportContext(order2);
        var contexts = new[] { context1, context2 };
        var duplicateStage = new DuplicateStage(NullLogger<DuplicateStage>.Instance);

        await duplicateStage.ExecuteAsync(contexts);

        Assert.Equal(ImportRecordStatus.Pending, context1.Status);
        Assert.Equal(ImportRecordStatus.Duplicate, context2.Status);
    }
}