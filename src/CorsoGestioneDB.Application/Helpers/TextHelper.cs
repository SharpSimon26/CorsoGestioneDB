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
        if (!string.IsNullOrWhiteSpace(value))
        {
            return new NormalizeResult<string?>(value, value.Trim().ToUpperInvariant());
        }
        else
        {
            return new NormalizeResult<string?>(value, null);
        }
    }

    public static NormalizeResult<string?> NormalizeLower(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return new NormalizeResult<string?>(value, value.Trim().ToLowerInvariant());
        }
        else
        {
            return new NormalizeResult<string?>(value, null);
        }
    }

    public static NormalizeResult<string?> NormalizeCapitalizeFirst(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var trimmedValue = value.Trim();
            string ucValue = char.ToUpper(trimmedValue[0]) + trimmedValue.Substring(1).ToLower();

            return new NormalizeResult<string?>(value, ucValue);  
        }
        else
        {
            return new NormalizeResult<string?>(value, null);
        }
    }
}