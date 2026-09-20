using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Reconstruction;

public interface IReconstructionRule
{
    Task<bool> CanApplyAsync(ImportContext context);
    Task ApplyAsync(ImportContext context);
}