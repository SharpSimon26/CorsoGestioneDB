using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
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

        // Traccia della modifica
        context.AddModification("DeliveryDate", calculatedDeliveryDate, order.DeliveryDate, GetType().Name, Stage.RECONSTRUCT);

        // Dato verosimile
        order.DeliveryDate = calculatedDeliveryDate;
    }
}