using System.Dynamic;

namespace CorsoGestioneDB.Application.Models;

public class ImportModification
{
    public string OrderID { get; private set; }
    public string Field { get; private set; }
    public string NewValue { get; private set; }
    public string OriginalValue { get; private set; }
    public string Message { get; private set; }
    public Stage Stage { get; private set; }

    public ImportModification(string orderId, string field, string newValue, string originalValue, 
                              string message, Stage stage)
    {
        OrderID = orderId;
        Field = field;
        NewValue = newValue;
        OriginalValue = originalValue;
        Message = message;
        Stage = stage;
    }
}