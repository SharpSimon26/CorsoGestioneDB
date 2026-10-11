using CorsoGestioneDB.Application.Helpers;

namespace CorsoGestioneDB.Application.Tests;

public class TextHelperTests
{
    [Theory]
    [InlineData("Pippo", "Pippo", false)]
    [InlineData(" Pippo ","Pippo", true)]
    [InlineData(null, null, false)]
    [InlineData("    ", null, true)]
    public void Normalize_Returns_String(string? text, string? atteso, bool changed)
    {
        var risultato = TextHelper.Normalize(text);
        Assert.Equal(text, risultato.OriginalValue);
        Assert.Equal(atteso, risultato.Value);
        Assert.Equal(changed, risultato.Changed);
    }

    [Theory]
    [InlineData("pippo", "PIPPO", true)]
    [InlineData(" Pippo ","PIPPO", true)]
    [InlineData(" PIPPO ","PIPPO", true)]
    [InlineData("PIPPO","PIPPO", false)]
    [InlineData(null, null, false)]
    [InlineData("    ", null, true)]
    public void NormalizeUpper_Returns_String(string? text, string? atteso, bool changed)
    {
        var risultato = TextHelper.NormalizeUpper(text);
        Assert.Equal(text, risultato.OriginalValue);
        Assert.Equal(atteso, risultato.Value);
        Assert.Equal(changed, risultato.Changed);
    }

    [Theory]
    [InlineData("PIPPO", "pippo", true)]
    [InlineData(" Pippo ","pippo", true)]
    [InlineData(" PIPPO ","pippo", true)]
    [InlineData("pippo","pippo", false)]
    [InlineData(null, null, false)]
    [InlineData("    ", null, true)]
    public void NormalizeLower_Returns_String(string? text, string? atteso, bool changed)
    {
        var risultato = TextHelper.NormalizeLower(text);
        Assert.Equal(text, risultato.OriginalValue);
        Assert.Equal(atteso, risultato.Value);
        Assert.Equal(changed, risultato.Changed);
    }

    [Theory]
    [InlineData("PIPPO", "Pippo", true)]
    [InlineData(" Pippo ","Pippo", true)]
    [InlineData(" PIPPO ","Pippo", true)]
    [InlineData("pippo","Pippo", true)]
    [InlineData("Pippo","Pippo", false)]
    [InlineData(null, null, false)]
    [InlineData("    ", null, true)]
    public void NormalizeCapitalizeFirst_Returns_String(string? text, string? atteso, bool changed)
    {
        var risultato = TextHelper.NormalizeCapitalizeFirst(text);
        Assert.Equal(text, risultato.OriginalValue);
        Assert.Equal(atteso, risultato.Value);
        Assert.Equal(changed, risultato.Changed);
    }
}