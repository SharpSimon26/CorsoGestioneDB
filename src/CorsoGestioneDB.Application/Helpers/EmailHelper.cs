using System.Net.Mail;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Helpers;

public static class EmailHelper
{
    public static NormalizeResult<string?> Normalize(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            return new NormalizeResult<string?>(email, email.Trim().ToLowerInvariant());
        }
        else
        {
            return new NormalizeResult<string?>(email, null);
        }
    }

    public static bool IsValid(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            try
            {
                return new MailAddress(email).Address == email;                
            }
            catch (Exception)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
