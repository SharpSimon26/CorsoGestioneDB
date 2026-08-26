using System.Diagnostics;
using CorsoGestioneDB.Application.Models;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Application.Engine;

[DebuggerDisplay("OrderID: {RawOrder.OrderID}, Status: {Status}, Messages: {Messages.Count}, Modifications: {Modifications.Count}, Issues: {Issues.Count}")]
public class ImportContext
{
    public StagingOrder RawOrder { get; private set; }
    public ImportData Data { get; private set; }
    public List<string> Messages { get; private set; }
    public List<ImportModification> Modifications { get; private set; }
    public List<ImportIssue> Issues { get; private set; }
    public ImportRecordStatus Status { get; private set; }
    public string? RejectReason { get; private set; }

    public ImportContext(StagingOrder rawOrder)
    {
        RawOrder = rawOrder;
        Status = ImportRecordStatus.Pending;
        Data = new();
        Messages = [];
        Modifications = [];
        Issues = [];
    }

    public void AddModification(string field, string? newValue, string? originalValue, string message, Stage stage)
    {
        Modifications.Add(new ImportModification(
            RawOrder.OrderID?.ToString() ?? "NULL",
            field, 
            newValue?.ToString() ?? "NULL",
            originalValue?.ToString() ?? "NULL",
            message,
            stage
        ));
    }

    public void AddModification(string field, int? newValue, int? originalValue, string message, Stage stage)
    {
        Modifications.Add(new ImportModification(
            RawOrder.OrderID?.ToString() ?? "NULL",
            field,
            newValue?.ToString() ?? "NULL",
            originalValue?.ToString() ?? "NULL",
            message,
            stage
        ));
    }

    public void AddModification(string field, decimal? newValue, decimal? originalValue, string message, Stage stage)
    {
        Modifications.Add(new ImportModification(
            RawOrder.OrderID?.ToString() ?? "NULL",
            field,
            newValue?.ToString() ?? "NULL",
            originalValue?.ToString() ?? "NULL",
            message,
            stage
        ));
    }

    public void AddModification(string field, DateTime? newValue, DateTime? originalValue, string message, Stage stage)
    {
        Modifications.Add(new ImportModification(
            RawOrder.OrderID?.ToString() ?? "NULL",
            field,
            newValue?.ToString() ?? "NULL",
            originalValue?.ToString() ?? "NULL",
            message,
            stage
        ));
    }

    public void AddIssue(string field, string message)
    {
        Issues.Add(new ImportIssue(field, message));
    }

    public bool IsProcessable()
    {
        return Status == ImportRecordStatus.Pending;
    }

    public bool IsRejected()
    {
        return Status == ImportRecordStatus.Rejected || 
               Status == ImportRecordStatus.Duplicate || 
               Status == ImportRecordStatus.Conflict;
    }

    public bool IsReady()
    {
        return Status == ImportRecordStatus.Ready;
    }

    public void MarkAsDuplicate(string? reason = null)
    {
        Status = ImportRecordStatus.Duplicate;
        RejectReason = reason;
    }

    public void MarkAsConflict(string? reason = null)
    {
        Status = ImportRecordStatus.Conflict;
        RejectReason = reason;
    }

    public void MarkAsRejected(string? reason = null)
    {
        Status = ImportRecordStatus.Rejected;
        RejectReason = reason;
    }
}
