using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

/// <summary>
/// Regola di verifica applicata qualora tutti i dati sembrino validi
/// </summary>
public class OrderLineDataIntegrityRule : IReconstructionRule
{
    private readonly ILogger<OrderLineDataIntegrityRule> _logger;

    public OrderLineDataIntegrityRule(ILogger<OrderLineDataIntegrityRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var line = context.Data.OrderLine;

        return line.Quantity.HasValue && line.Quantity > 0 &&
               line.UnitPrice.HasValue && line.UnitPrice > 0 &&
               line.DiscountPct.HasValue && line.DiscountPct >= 0 &&
               line.ShippingCost.HasValue && line.ShippingCost >= 0 &&              
               line.Revenue.HasValue && line.Revenue > 0;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var quantity = line.Quantity.GetValueOrDefault();
        var unitPrice = line.UnitPrice.GetValueOrDefault();
        var discountPct = line.DiscountPct.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();
        var revenue = line.Revenue.GetValueOrDefault();

        // Prezzo unitario scontato
        var netUnitPrice = unitPrice * (1 - (discountPct / 100m));
    
        // Incasso calcolato
        var calculatedRevenue = Math.Round(
            (quantity * netUnitPrice) + shippingCost, 2, MidpointRounding.AwayFromZero
        );

        // I dati non sono coerenti, il record viene rigettato
        if (calculatedRevenue != revenue)
        {
            context.MarkAsRejected("I dati dell'ordine non sono coerenti e non possono essere importati nel database");
        }
    }
}