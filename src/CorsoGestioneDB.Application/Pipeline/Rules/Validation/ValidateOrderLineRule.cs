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

        if (orderLine.Quantity == null || orderLine.Quantity <= 0)
        {
            context.AddIssue(nameof(orderLine.Quantity), $"{nameof(orderLine.Quantity)} è '{orderLine.Quantity}'");
        }

        if (orderLine.UnitPrice == null || !(orderLine.UnitPrice > 0))
        {
            context.AddIssue(nameof(orderLine.UnitPrice), $"{nameof(orderLine.UnitPrice)} è '{orderLine.UnitPrice}'");
        }

        if (orderLine.DiscountPct == null || orderLine.DiscountPct < 0)
        {
            context.AddIssue(nameof(orderLine.DiscountPct), $"{nameof(orderLine.DiscountPct)} è '{orderLine.DiscountPct}'");
        }

        if (orderLine.ShippingCost == null || orderLine.ShippingCost < 0)
        {
            context.AddIssue(nameof(orderLine.ShippingCost), $"{nameof(orderLine.ShippingCost)} è '{orderLine.ShippingCost}'");
        }

        if (orderLine.Revenue == null || orderLine.Revenue < 0)
        {
            context.AddIssue(nameof(orderLine.Revenue), $"{nameof(orderLine.Revenue)} è '{orderLine.Revenue}'");
        }
    }
}
