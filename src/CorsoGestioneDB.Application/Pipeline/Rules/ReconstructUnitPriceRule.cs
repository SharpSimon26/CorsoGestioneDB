using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

/// <summary>
/// Regola di ricostruzione applicata qualora gli altri dati appaiano coerenti
/// ma il prezzo unitario sia minore o uguale a 0
/// </summary>
public class ReconstructUnitPriceRule : IReconstructionRule
{
    private readonly ILogger<ReconstructUnitPriceRule> _logger;

    public ReconstructUnitPriceRule(ILogger<ReconstructUnitPriceRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var line = context.Data.OrderLine;

        return line.Quantity.HasValue && line.Quantity > 0 &&
               (line.UnitPrice == null || line.UnitPrice <= 0) && // Rileva un dato errato
               line.DiscountPct.HasValue && line.DiscountPct >= 0 &&
               line.ShippingCost.HasValue && line.ShippingCost >= 0 &&
               line.Revenue.HasValue && line.Revenue > 0;
    }
    
    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var quantity = line.Quantity.GetValueOrDefault();
        var discountPct = line.DiscountPct.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();
        var revenue = line.Revenue.GetValueOrDefault();

        // Quantità di articoli se comprati a prezzo pieno
        var discountFactor = quantity * (1 - (discountPct / 100m));

        // Calcolo del prezzo unitario 
        var calculatedUnitPrice = Math.Round(
            (revenue - shippingCost) / discountFactor, 2, MidpointRounding.AwayFromZero
        );

        var msg = string.Format("UnitPrice modificato in {0} valore originale {1}", calculatedUnitPrice, line.UnitPrice);
        context.Messages.Add(msg);
        _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

        line.UnitPrice = calculatedUnitPrice;
    }
}