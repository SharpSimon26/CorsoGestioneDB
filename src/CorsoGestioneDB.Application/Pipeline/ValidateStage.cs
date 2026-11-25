using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules.Validation;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class ValidateStage : StageBase
{
    private readonly IEnumerable<IValidationRule> _rules;

    public ValidateStage(IEnumerable<IValidationRule> rules, ILogger<ValidateStage> logger) : base(logger)
    {
        _rules = rules;
    }

    public override async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        // Ciclo per tutte le righe
        foreach (var context in contexts.Where(x => x.IsProcessable()))
        {
            // Ciclo per tutte le regole applicabili alla riga
            foreach (var rule in _rules)
            {
                if (rule.CanApply(context))
                {
                    await rule.ApplyAsync(context);
                }
            }
        }

        throw new NotImplementedException();
    }
}