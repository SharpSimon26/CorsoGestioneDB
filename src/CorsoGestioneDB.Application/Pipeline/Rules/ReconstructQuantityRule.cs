using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

/// <summary>
/// Regola di ricostruzione applicata qualora gli altri dati appaiano coerenti ma la quantità sia minore o uguale a 0
/// </summary>
public class ReconstructQuantityRule : IReconstructionRule
{
    private readonly ILogger<ReconstructQuantityRule> _logger;

    public ReconstructQuantityRule(ILogger<ReconstructQuantityRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var line = context.Data.OrderLine;

        return line.Revenue.HasValue && line.Revenue > 0 &&
               (line.Quantity == null || line.Quantity <= 0) && // Rileva un dato errato
               line.UnitPrice.HasValue && line.UnitPrice > 0 &&
               line.DiscountPct.HasValue && line.DiscountPct >= 0 &&
               line.ShippingCost.HasValue && line.ShippingCost >= 0;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var revenue = line.Revenue.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();
        var unitPrice = line.UnitPrice.GetValueOrDefault();
        var discountPct = line.DiscountPct.GetValueOrDefault();

        var netUnitPrice = unitPrice * (1 - (discountPct / 100m)); // prezzo unitario scontato
        var calculatedQuantity = (int)Math.Round(
            (revenue - shippingCost) / netUnitPrice, 0, MidpointRounding.AwayFromZero
        );

        var msg = string.Format("Quantity modificato in {0} valore originale {1}", calculatedQuantity, line.Quantity);
        context.Messages.Add(msg);
        _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

        line.Quantity = calculatedQuantity;
    }
}