using CorsoGestioneDB.Application.Models;
using System.Globalization;

namespace CorsoGestioneDB.Application.Helpers;

public static class ConvertHelper
{
    public static ConvertResult<int> ToInt(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ConvertResult<int>(value, null, "La conversione in int è fallita, il valore è Null o vuoto");
        }

        var cleanedValue = value.Trim();

        if (int.TryParse(cleanedValue, CultureInfo.InvariantCulture, out int intValue))
        {
            return new ConvertResult<int>(value, intValue);
        }
        else
        {
            return new ConvertResult<int>(value, null, $"La conversione di '{value}' in int è fallita");
        }
    }

    public static ConvertResult<decimal> ToDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ConvertResult<decimal>(value, null, "La conversione in decimal è fallita, il valore è Null o vuoto");
        }

        var cleanedValue = value.Trim();
        var cultureIt = CultureInfo.GetCultureInfo("it-IT"); // cultura italiana -> supporta il . per le migliaia
        var cultureInv = CultureInfo.InvariantCulture; // cultura internazionale , per i decimali
        CultureInfo cultureToUse;

        int lastDot = cleanedValue.LastIndexOf('.');
        int lastComma = cleanedValue.LastIndexOf(',');

        if (lastComma > lastDot)
        {
            cultureToUse = cultureIt;
        }
        else
        {
            cultureToUse = cultureInv;
        }

        if (decimal.TryParse(value, NumberStyles.Any, cultureToUse, out decimal decValue))
        {
            return new ConvertResult<decimal>(value, decValue);
        }
        else
        {
            return new ConvertResult<decimal>(value, null, $"La conversione di '{value}' in decimal è fallita. Formato non supportato.");
        }
    }

    public static ConvertResult<DateTime> ToDateTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ConvertResult<DateTime>(value, null, "La conversione in DateTime è fallita, il valore è Null o vuoto");
        }

        var cleanedValue = value.Trim();
        string[] formatiSupportati =
        [
            "dd/MM/yyyy",
            "dd/MM/yyyy HH:mm:ss",
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm:ss",
            "dd-MM-yyyy",
            "yyyy/MM/dd"
        ];

        if (DateTime.TryParseExact(cleanedValue, formatiSupportati, CultureInfo.InvariantCulture, 
                                    DateTimeStyles.None, out DateTime dateValue))
        {
            return new ConvertResult<DateTime>(value, dateValue);
        }
        else
        {
            return new ConvertResult<DateTime>(value, null, $"La conversione di '{value}' in DateTime è fallita. Formato non supportato.");
        }
    }
}
