using System.Diagnostics;
using CorsoGestioneDB.Application.Models;
using CorsoGestioneDB.Domain.Entities;

namespace CorsoGestioneDB.Application.Engine;

[DebuggerDisplay("OrderID: {RawOrder.OrderID}, Status: {Status}, Messages: {Messages.Count}")]
public class ImportContext
{
    public StagingOrder RawOrder { get; private set; }
    public ImportData Data { get; private set; }
    public List<string> Messages { get; private set; }
    public ImportRecordStatus Status { get; private set; }
    public string? RejectReason { get; private set; }

    public ImportContext(StagingOrder rawOrder)
    {
        RawOrder = rawOrder;
        Status = ImportRecordStatus.Pending;
        Data = new();
        Messages = [];
    }

    public bool IsProcessable()
    {
        return Status == ImportRecordStatus.Pending;
    }

    public bool IsRejected()
    {
        return Status == ImportRecordStatus.Rejected || Status == ImportRecordStatus.Duplicate || Status == ImportRecordStatus.Conflict;
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
