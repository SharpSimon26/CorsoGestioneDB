using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class ImportStage : StageBase
{
    public ImportStage(ILogger<ImportStage> logger) : base(logger)
    {
    }

    public override async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        foreach (var context in contexts.Where(x => x.IsReady()))
        {
            // TODO ...
        }

        throw new NotImplementedException();
    }
}