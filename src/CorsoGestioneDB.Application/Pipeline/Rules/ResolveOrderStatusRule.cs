using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ResolveOrderStatusRule : IResolutionRule
{
    private readonly ICachedOrderStatusRepository _orderStatusRepository;
    private readonly ILogger<ResolveOrderStatusRule> _logger;

    /// <summary>
    /// Regola di risoluzione applicata a OrderStatus
    /// </summary>
    public ResolveOrderStatusRule(ICachedOrderStatusRepository orderStatusRepository, ILogger<ResolveOrderStatusRule> logger)
    {
        _orderStatusRepository = orderStatusRepository;
        _logger = logger;
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

        var status = await _orderStatusRepository.GetByNameAsync(order.OrderStatus!);

        if (status != null)
        {
            order.OrderStatusID = status.OrderStatusID;
        }
        else
        {
            // add issue
        }
    }
}