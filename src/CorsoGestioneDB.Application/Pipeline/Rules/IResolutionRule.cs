using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public interface IResolutionRule
{
    bool CanApply(ImportContext context);
    Task ApplyAsync(ImportContext context);
}