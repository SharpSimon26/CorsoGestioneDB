using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public abstract class StageBase : IStage
{
    protected readonly ILogger logger;

    protected StageBase(ILogger logger)
    {
        this.logger = logger;
    }

    public abstract Task ExecuteAsync(IEnumerable<ImportContext> contexts);
}
