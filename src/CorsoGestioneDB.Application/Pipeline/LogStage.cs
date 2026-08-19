using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class LogStage : StageBase
{
    public LogStage(ILogger<LogStage> logger) : base(logger)
    {
    }

    public override async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        throw new NotImplementedException();
    }
}