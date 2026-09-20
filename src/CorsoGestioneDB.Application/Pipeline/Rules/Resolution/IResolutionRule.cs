using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Resolution;

public interface IResolutionRule
{
    bool CanApply(ImportContext context);
    Task ApplyAsync(ImportContext context);
}