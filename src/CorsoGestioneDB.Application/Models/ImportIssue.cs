namespace CorsoGestioneDB.Application.Models;

public class ImportIssue
{
    public string Field { get; private set; }
    public string Message { get; private set; }

    public ImportIssue(string field, string message)
    {
        Field = field;
        Message = message;
    }
}
