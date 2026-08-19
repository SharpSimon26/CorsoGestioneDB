namespace CorsoGestioneDB.Application.Models;

public class ConvertResult<T> where T : struct
{
    public T? Value { get; private set; }
    public string? OriginalValue { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public bool Success 
    { 
        get => Value != null;
    }

    public ConvertResult(string? originalValue, T? convertedValue)
    {
        Value = convertedValue;
        OriginalValue = originalValue;
    }

    public ConvertResult(string? originalValue, T? convertedValue, string errorMessage)
    {
        Value = convertedValue;
        OriginalValue = originalValue;
        ErrorMessage = errorMessage;
    }
}