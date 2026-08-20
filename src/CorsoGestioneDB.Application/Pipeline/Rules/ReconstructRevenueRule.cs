using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

/// <summary>
/// Regola di ricostruzione applicata qualora gli altri dati appaiano coerenti
/// ma Revenue sia minore o uguale a 0
/// </summary>
public class ReconstructRevenueRule : IReconstructionRule
{
    private readonly ILogger<ReconstructRevenueRule> _logger;

    public ReconstructRevenueRule(ILogger<ReconstructRevenueRule> logger)
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
               (line.Revenue == null || line.Revenue <= 0); // Rileva un dato errato
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var quantity = line.Quantity.GetValueOrDefault();
        var unitPrice = line.UnitPrice.GetValueOrDefault();
        var discountPct = line.DiscountPct.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();

        // Prezzo unitario scontato
        var netUnitPrice = unitPrice * (1 - (discountPct / 100m));

        // Calcolo Revenue
        var calculatedRevenue = Math.Round(
            (quantity * netUnitPrice) + shippingCost, 2, MidpointRounding.AwayFromZero
        );

        var msg = string.Format("Revenue modificato in {0} valore originale {1}", calculatedRevenue, line.Revenue);
        context.Messages.Add(msg);
        _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

        // Dato corretto
        line.Revenue = calculatedRevenue;
    }
}