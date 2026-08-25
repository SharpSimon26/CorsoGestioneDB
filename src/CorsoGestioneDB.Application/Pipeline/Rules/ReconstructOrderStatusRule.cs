using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

/// <summary>
/// Regola di ricostruzione applicata a OrderStatus
/// </summary>
public class ReconstructOrderStatusRule : IReconstructionRule
{
    private readonly ILogger<ReconstructOrderStatusRule> _logger;

    public ReconstructOrderStatusRule(ILogger<ReconstructOrderStatusRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var order = context.Data.Order;

        return order.OrderStatus == null || 
               order.OrderStatus.Equals("Unknown", StringComparison.OrdinalIgnoreCase) ||
               (order.OrderStatus.Equals("In lavorazione", StringComparison.OrdinalIgnoreCase) && order.DeliveryDate != null) ||
               (order.OrderStatus.Equals("Spedito", StringComparison.OrdinalIgnoreCase) && order.DeliveryDate != null);
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var order = context.Data.Order;

        string calculatedOrderStatus;
       
        if (order.OrderStatus == null && order.DeliveryDate == null)
        {
            calculatedOrderStatus = "Unknown";

            var msg = string.Format("OrderStatus modificato in {0} valore originale {1}", calculatedOrderStatus, order.OrderStatus);
            context.Messages.Add(msg);
            _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

            // Dato corretto
            order.OrderStatus = calculatedOrderStatus;
        }
        else if (order.DeliveryDate != null)
        {
            calculatedOrderStatus = "Consegnato";

            var msg = string.Format("OrderStatus modificato in {0} valore originale {1}", calculatedOrderStatus, order.OrderStatus);
            context.Messages.Add(msg);
            _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

            // Dato corretto
            order.OrderStatus = calculatedOrderStatus;
        }
    }
}