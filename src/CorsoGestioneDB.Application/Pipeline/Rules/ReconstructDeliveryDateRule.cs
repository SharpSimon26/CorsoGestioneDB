using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;


public class ReconstructDeliveryDateRule : IReconstructionRule
{
    private readonly ILogger<ReconstructDeliveryDateRule> _logger;

    /// <summary>
    /// Regola di ricostruzione applicata a DeliveryDate qualora sia
    /// precedente ad OrderDate.
    /// Il campo DeliveryDate viene valorizzato con un dato verosimile 
    /// di 4 giorni successivi a OrderDate
    /// </summary>
    public ReconstructDeliveryDateRule(ILogger<ReconstructDeliveryDateRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var order = context.Data.Order;
        return order.OrderDate != null && order.DeliveryDate != null &&
               order.DeliveryDate < order.OrderDate;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var order = context.Data.Order;
        var orderDate = order.OrderDate.GetValueOrDefault();

        var calculatedDeliveryDate = orderDate.AddDays(4);

        var msg = string.Format("DeliveryDate modificato in {0} valore originale {1}", calculatedDeliveryDate.ToString(), order.DeliveryDate?.ToString() ?? "NULL");
        context.Messages.Add(msg);
        _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

        // Dato verosimile
        order.DeliveryDate = calculatedDeliveryDate;
    }
}