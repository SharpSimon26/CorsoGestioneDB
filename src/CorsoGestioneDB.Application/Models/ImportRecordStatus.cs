namespace CorsoGestioneDB.Application.Models;

public enum ImportRecordStatus
{
    /// <summary>
    /// In fase di elaborazione
    /// </summary>
    Pending,

    /// <summary>
    /// Record duplicato, non verrà importato
    /// </summary>
    Duplicate,

    /// <summary>
    /// Campo chiave duplicato, non verrà importato
    /// </summary>
    Conflict,

    /// <summary>
    /// Errore di validazione o conversione non risolvibile, non verrà importato
    /// </summary>
    Rejected,

    /// <summary>
    /// Record normalizzato convertito e validato con successo, può importato nel database
    /// </summary>
    Ready
}