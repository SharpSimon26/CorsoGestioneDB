using CorsoGestioneDB.Application.Engine;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Validation;

public class ValidateOrderRule : IValidationRule
{
    public bool CanApply(ImportContext context)
    {
        return true;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        // Order
        var order = context.Data.Order;

        if (string.IsNullOrWhiteSpace(order.OrderID))
        {
            context.AddIssue(nameof(order.OrderID), $"{nameof(order.OrderID)} è null");
        }

        if (order.OrderDate == null)
        {
            context.AddIssue(nameof(order.OrderDate), $"{nameof(order.OrderDate)} è null");
        }

        if (order.DeliveryDate == null && order.OrderStatusID == 2) // Consegnato
        {
            context.AddIssue(nameof(order.DeliveryDate), $"Ordine {order.OrderStatus} ma {nameof(order.DeliveryDate)} è null");
        }
    }
}
