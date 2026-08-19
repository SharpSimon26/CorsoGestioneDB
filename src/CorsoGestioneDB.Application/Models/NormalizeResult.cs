namespace CorsoGestioneDB.Application.Models;

public class NormalizeResult<T> where T : IComparable<T>?
{
    public T? Value { get; private set; }
    public T? OriginalValue { get; private set; }
    public bool Changed { get; private set; }
    public string Message { get; private set; } = string.Empty;

    public NormalizeResult(T? originalValue, T? normalizedValue)
    {
        Value = normalizedValue;
        OriginalValue = originalValue;

        // Confronta i due valori gestendo correttamente anche i tipi null
        Changed = !EqualityComparer<T>.Default.Equals(originalValue, normalizedValue);
    }

    public NormalizeResult(T? originalValue, T? normalizedValue, string message)
    {
        Value = normalizedValue;
        OriginalValue = originalValue;

        // Confronta i due valori gestendo correttamente anche i tipi null
        Changed = !EqualityComparer<T>.Default.Equals(originalValue, normalizedValue);

        Message = message;
    }
}