using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructOrderDateRule : IReconstructionRule
{
    /// <summary>
    /// Regola di ricostruzione applicata a OrderDate qualora sia NULL 
    /// e DeliveryDate sia valorizzato.
    /// Essendo NULL, il campo OrderDate viene valorizzato con un dato 
    /// verosimile di 4 giorni prima della DeliveryDate
    /// </summary>
    public ReconstructOrderDateRule()
    {
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

        // Traccia della modifica
        context.AddModification("OrderDate", calculatedOrderDate, order.OrderDate, GetType().Name, Stage.RECONSTRUCT);

        // Dato verosimile
        order.OrderDate = calculatedOrderDate;
    }
}