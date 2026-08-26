namespace CorsoGestioneDB.Application.Models;

public enum Stage
{
    NORMALIZE,
    DUPLICATE,
    CONVERT,
    RECONSTRUCT,
    RESOLVE,
    VALIDATE,
    IMPORT,
    UNSPECIFIED
}