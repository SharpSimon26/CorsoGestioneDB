using CorsoGestioneDB.Application.Helpers;

namespace CorsoGestioneDB.Application.Tests.Helpers;

public class ConverHelperTests
{
    [Theory]
    [InlineData("13", 13, true)]
    [InlineData("   47  ", 47, true)]
    [InlineData(null, null, false)]
    [InlineData("   ", null, false)]
    [InlineData("dieci", null, false)] 
    public void Convert_String_Returns_Integer(string? value, int? atteso, bool success)
    {
        var risultato = ConvertHelper.ToInt(value);
        Assert.Equal(risultato.Success, success);
        Assert.Equal(value, risultato.OriginalValue);
        Assert.Equal(atteso, risultato.Value);
    }

    public static TheoryData<string?, decimal?, bool> ConvertToDecimalData = new()
    {
        { "10.3", 10.3m, true },
        { " 113.71 ", 113.71m, true },
        { "1.234,50", 1234.50m, true },
        { "13,71", 13.71m, true },
        { null, null, false },
        { "dieci", null, false }
    };

    [Theory]
    [MemberData(nameof(ConvertToDecimalData))]   
    public void Convert_String_Returns_Decimal(string? value, decimal? atteso, bool success)
    {
        var risultato = ConvertHelper.ToDecimal(value);

        Assert.Equal(success, risultato.Success);
        Assert.Equal(atteso, risultato.Value);
    }

    public static TheoryData<string?, DateTime?, bool> ConvertToDateTimeData = new()
    {
        { "2026-08-09", new DateTime(2026, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), true },
        { "09/08/2026", new DateTime(2026, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), true },
        { "09/08/2026 14:30:00", new DateTime(2026, 8, 9, 14, 30, 0, DateTimeKind.Unspecified), true },
        { "2025-13-10", null, false },
        { null, null, false },
        { "   ", null, false },
        { "testo_non_valido", null, false }
    };

    [Theory]
    [MemberData(nameof(ConvertToDateTimeData))]
    public void Convert_String_Returns_DateTime(string? value, DateTime? atteso, bool success)
    {
        var risultato = ConvertHelper.ToDateTime(value);
        Assert.Equal(success, risultato.Success);
        Assert.Equal(atteso, risultato.Value);
    }
}
