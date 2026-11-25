using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Validation;

public class ValidateForeignKeysRule : IValidationRule
{
    public bool CanApply(ImportContext context)
    {
        return true;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        // Order
        var order = context.Data.Order;

        if (order.OrderStatusID == null)
        {
            context.AddIssue(nameof(order.OrderStatusID), "OrderStatusID non risolto");
        }

        // Customer
        var customer = context.Data.Customer;

        if (customer.CustomerID == null)
        {
            context.AddIssue(nameof(customer.CustomerID), "CustomerID non risolto");
        }
    }
}
