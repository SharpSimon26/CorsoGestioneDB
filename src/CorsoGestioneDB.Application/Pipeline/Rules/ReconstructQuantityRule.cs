using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructQuantityRule : IReconstructionRule
{
    private readonly ILogger<ReconstructQuantityRule> _logger;

    /// <summary>
    /// Regola di ricostruzione applicata qualora gli altri dati appaiano coerenti
    /// ma la quantità sia minore o uguale a 0
    /// </summary>
    public ReconstructQuantityRule(ILogger<ReconstructQuantityRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var line = context.Data.OrderLine;

        return (line.Quantity == null || line.Quantity <= 0) && // Rileva un dato errato
               line.UnitPrice.HasValue && line.UnitPrice > 0 &&
               line.DiscountPct.HasValue && line.DiscountPct >= 0 &&
               line.ShippingCost.HasValue && line.ShippingCost >= 0 &&
               line.Revenue.HasValue && line.Revenue > 0;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var unitPrice = line.UnitPrice.GetValueOrDefault();
        var discountPct = line.DiscountPct.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();
        var revenue = line.Revenue.GetValueOrDefault();

        // Prezzo unitario scontato
        var netUnitPrice = unitPrice * (1 - (discountPct / 100m));

        // Calcolo della quantità in formato int
        var calculatedQuantity = (int)Math.Round(
            (revenue - shippingCost) / netUnitPrice, 0, MidpointRounding.AwayFromZero
        );

        // Traccia della modifica
        context.AddModification("Quantity", calculatedQuantity, line.Quantity, GetType().Name, Stage.RECONSTRUCT);

        // Dato corretto
        line.Quantity = calculatedQuantity;
    }
}