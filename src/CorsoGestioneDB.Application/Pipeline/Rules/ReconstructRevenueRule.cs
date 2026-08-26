using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructRevenueRule : IReconstructionRule
{
    private readonly ILogger<ReconstructRevenueRule> _logger;

/// <summary>
/// Regola di ricostruzione applicata qualora gli altri dati appaiano coerenti
/// ma Revenue sia minore o uguale a 0
/// </summary>
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

        context.AddModification("Revenue", calculatedRevenue, line.Revenue, GetType().Name, Stage.RECONSTRUCT);

        // Dato corretto
        line.Revenue = calculatedRevenue;
    }
}