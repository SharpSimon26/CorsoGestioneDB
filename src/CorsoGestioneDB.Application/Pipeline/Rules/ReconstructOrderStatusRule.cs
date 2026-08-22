using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

/// <summary>
/// Regola di ricostruzione applicata qualora OrderStatus sia NULL o Unknown
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

        return order.OrderStatus == null || order.OrderStatus == "Unknown";
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var order = context.Data.Order;
       
        if (order.OrderStatus == null && order.DeliveryDate == null)
        {
            var calculatedOrderStatus = "Unknown";

            var msg = string.Format("OrderStatus modificato in {0} valore originale {1}", calculatedOrderStatus, order.OrderStatus);
            context.Messages.Add(msg);
            _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

            // Dato correto
            order.OrderStatus = calculatedOrderStatus;
        }
        else if (order.DeliveryDate != null)
        {
            var calculatedOrderStatus = "Consegnato";

            var msg = string.Format("OrderStatus modificato in {0} valore originale {1}", calculatedOrderStatus, order.OrderStatus);
            context.Messages.Add(msg);
            _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

            // Dato corretto
            order.OrderStatus = calculatedOrderStatus;
        }
    }
}