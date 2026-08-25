using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class ResolveStage : StageBase
{
    private readonly IEnumerable<IResolutionRule> _rules;

    public ResolveStage(IEnumerable<IResolutionRule> rules, ILogger<ResolveStage> logger) : base(logger)
    {
        _rules = rules;
    }

    public override async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        // Ciclo per tutte le righe
        foreach (var context in contexts.Where(x => x.IsProcessable()))
        {
            // Ciclo per tutte le regole applicabili alla riga
            foreach (var rule in _rules.Where(r => r.CanApply(context)))
            {
                await rule.ApplyAsync(context);
            }
        }
    }
}