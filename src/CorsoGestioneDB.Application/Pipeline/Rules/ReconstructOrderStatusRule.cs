using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructOrderStatusRule : IReconstructionRule
{
    /// <summary>
    /// Regola di ricostruzione applicata a OrderStatus
    /// </summary>
    public ReconstructOrderStatusRule()
    {
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
        }
        else if (order.DeliveryDate != null)
        {
            calculatedOrderStatus = "Consegnato";
        }
        else
        {
            return;
        }

        // Traccia della modifica
        context.AddModification("OrderStatus", calculatedOrderStatus, order.OrderStatus, GetType().Name, Stage.RECONSTRUCT);

        // Dato corretto
        order.OrderStatus = calculatedOrderStatus;        
    }
}