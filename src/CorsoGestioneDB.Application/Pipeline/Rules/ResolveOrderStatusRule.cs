using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ResolveOrderStatusRule : IResolutionRule
{
    private readonly ICachedOrderStatusRepository _orderStatusRepository;

    /// <summary>
    /// Regola di risoluzione applicata a OrderStatusID
    /// </summary>
    public ResolveOrderStatusRule(ICachedOrderStatusRepository orderStatusRepository)
    {
        _orderStatusRepository = orderStatusRepository;
    }

    public bool CanApply(ImportContext context)
    {
        var order = context.Data.Order;
    
        return !string.IsNullOrWhiteSpace(order.OrderStatus) &&
               order.OrderStatusID == null;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var order = context.Data.Order;

        // Recupera lo stato dell'ordine dal database
        var status = await _orderStatusRepository.GetByNameAsync(order.OrderStatus!);

        if (status != null)
        {
            context.AddModification(nameof(order.OrderStatusID), status.OrderStatusID, order.OrderStatusID, "Database lookup", Stage.RESOLVE);
            order.OrderStatusID = status.OrderStatusID;
        }
        else
        {
            context.AddIssue(nameof(order.OrderStatus), $"Stato '{order.OrderStatus}' non trovato.");
        }
    }
}