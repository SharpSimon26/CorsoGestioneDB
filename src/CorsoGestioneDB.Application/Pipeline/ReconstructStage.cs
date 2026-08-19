using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class ReconstructStage : StageBase
{
    private readonly IEnumerable<IReconstructionRule> _rules;

    public ReconstructStage(IEnumerable<IReconstructionRule> rules, ILogger<ReconstructStage> logger) : base(logger)
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