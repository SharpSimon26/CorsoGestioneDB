using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Validation;

public class ValidateOrderLineRule : IValidationRule
{
    public bool CanApply(ImportContext context)
    {
        return true;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var orderLine = context.Data.OrderLine;

        if (orderLine.Quantity == null || orderLine.Quantity < 1)
        {
            context.AddIssue(nameof(orderLine.Quantity), $"{nameof(orderLine.Quantity)} è '{orderLine.Quantity}'");
        }

        if (orderLine.DiscountPct == null || orderLine.DiscountPct < 0)
        {
            context.AddIssue(nameof(orderLine.Quantity), $"{nameof(orderLine.Quantity)} è '{orderLine.Quantity}'");
        }
    }
}
