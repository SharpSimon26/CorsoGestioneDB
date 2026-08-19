using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Helpers;

public static class TextHelper
{
    public static NormalizeResult<string?> Normalize(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return new NormalizeResult<string?>(value, value.Trim());
        }
        else
        {
            return new NormalizeResult<string?>(value, null);
        }
    }

    public static NormalizeResult<string?> NormalizeUpper(string? value)
    {
        var trimmedValue = Normalize(value).Value;

        if (trimmedValue != null)
        {
            return new NormalizeResult<string?>(value, trimmedValue.ToUpperInvariant());
        }
        else
        {
            return new NormalizeResult<string?>(trimmedValue, null);
        }
    }

    public static NormalizeResult<string?> NormalizeLower(string? value)
    {
        var trimmedValue = Normalize(value).Value;

        if (trimmedValue != null)
        {
            return new NormalizeResult<string?>(value, trimmedValue.ToLowerInvariant());
        }
        else
        {
            return new NormalizeResult<string?>(value, null);
        }
    }
}