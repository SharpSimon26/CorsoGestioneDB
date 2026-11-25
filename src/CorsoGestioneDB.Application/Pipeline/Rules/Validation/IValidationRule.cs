using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Validation;

public interface IValidationRule
{
    bool CanApply(ImportContext context);
    Task ApplyAsync(ImportContext context);
}