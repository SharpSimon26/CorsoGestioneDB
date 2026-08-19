using CorsoGestioneDB.Application.Helpers;

namespace CorsoGestioneDB.Application.Tests;

public class EmailHelperTests
{
    [Theory]
    [InlineData("pippo@pluto.com", "pippo@pluto.com", false)]
    [InlineData("  pippo@pluto.com  ", "pippo@pluto.com", true)]
    [InlineData(" PIPPO@PLUTO.com  ", "pippo@pluto.com", true)]
    [InlineData(null, null, false)]
    [InlineData("      ", null, true)]
    public void Normalize_Email_Returns_String(string? email, string? atteso, bool changed)
    {
        var risultato = EmailHelper.Normalize(email);
        Assert.Equal(changed, risultato.Changed);
        Assert.Equal(email, risultato.OriginalValue);
        Assert.Equal(atteso, risultato.Value);
    }
}