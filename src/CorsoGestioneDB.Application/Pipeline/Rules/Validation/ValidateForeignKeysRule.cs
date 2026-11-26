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

        if (order.PaymentMethodID == null)
        {
            context.AddIssue(nameof(order.PaymentMethodID), $"{nameof(order.PaymentMethodID)} non risolto");
        }

        if (order.SalesChannelID == null)
        {
            context.AddIssue(nameof(order.SalesChannelID), $"{nameof(order.SalesChannelID)} non risolto");
        }

        if (order.OrderStatusID == null)
        {
            context.AddIssue(nameof(order.OrderStatusID), $"{nameof(order.OrderStatusID)} non risolto");
        }

        // Customer
        var customer = context.Data.Customer;

        if (customer.CustomerID == null)
        {
            context.AddIssue(nameof(customer.CustomerID), $"{nameof(customer.CustomerID)} non risolto");
        }

        if (customer.CityID == null)
        {
            context.AddIssue(nameof(customer.CityID), $"{nameof(customer.CityID)} non risolto");
        }

        // Product
        var product = context.Data.Product;

        if (product.ProductCode == null)
        {
            context.AddIssue(nameof(product.ProductCode), $"{nameof(product.ProductCode)} non risolto");
        }

        if (product.CategoryID == null)
        {
            context.AddIssue(nameof(product.CategoryID), $"{nameof(product.CategoryID)} non risolto");
        }
    }
}
