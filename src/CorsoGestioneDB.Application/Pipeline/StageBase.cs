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

    public virtual void LogModifications(IEnumerable<ImportContext> contexts)
    {
        foreach (var context in contexts)
        {
            LogModifications(context);
        }
    }

    public virtual void LogModifications(ImportContext context)
    {
        foreach (var item in context.Modifications)
        {
            logger.LogInformation("Ordine: '{0}' Campo: '{1}' Nuovo valore: '{2}' Valore originale: '{3}' Info: '{4}' Stage: '{5}'",
                item.OrderID, item.Field, item.NewValue, item.OriginalValue, item.Message, item.Stage
            );
        }
    }
}
