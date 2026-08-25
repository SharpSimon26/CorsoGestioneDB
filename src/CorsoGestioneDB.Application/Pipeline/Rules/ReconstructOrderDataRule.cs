using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructOrderDateRule : IReconstructionRule
{
    private readonly ILogger<ReconstructOrderDateRule> _logger;

    /// <summary>
    /// Regola di ricostruzione applicata a OrderDate qualora sia NULL 
    /// e DeliveryDate sia valorizzato.
    /// Essendo NULL, il campo OrderDate viene valorizzato con un dato 
    /// verosimile di 4 giorni prima della DeliveryDate
    /// </summary>
    public ReconstructOrderDateRule(ILogger<ReconstructOrderDateRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var order = context.Data.Order;

        return order.OrderDate == null && order.DeliveryDate != null;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var order = context.Data.Order;
        var deliveryDate = order.DeliveryDate.GetValueOrDefault();

        var calculatedOrderDate = deliveryDate.Subtract(TimeSpan.FromDays(4));

        var msg = string.Format("OrderDate modificato in {0} valore originale {1}", calculatedOrderDate.ToString(), order.OrderDate?.ToString() ?? "NULL");
        context.Messages.Add(msg);
        _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

        // Dato verosimile
        order.OrderDate = calculatedOrderDate;
    }
}