using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Reconstruction;

public interface IReconstructionRule
{
    bool CanApply(ImportContext context);
    Task ApplyAsync(ImportContext context);
}