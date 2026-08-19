using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructDiscountPctRule : IReconstructionRule
{
    private readonly ILogger<ReconstructDiscountPctRule> _logger;

    public ReconstructDiscountPctRule(ILogger<ReconstructDiscountPctRule> logger)
    {
        _logger = logger;
    }

    public bool CanApply(ImportContext context)
    {
        var line = context.Data.OrderLine;

        return line.Revenue.HasValue && line.Revenue > 0 &&
               line.Quantity.HasValue && line.Quantity > 0 &&
               line.UnitPrice.HasValue && line.UnitPrice > 0 &&
               (line.DiscountPct == null || line.DiscountPct < 0 || line.DiscountPct > 99) &&
               line.ShippingCost.HasValue && line.ShippingCost >= 0;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var revenue = line.Revenue.GetValueOrDefault();
        var quantity = line.Quantity.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();
        var unitPrice = line.UnitPrice.GetValueOrDefault();

        // Prezzo totale non scontato della merce
        var grossTotal = quantity * unitPrice;

        // Incasso senza spese di spedizione
        var netRevenue = revenue - shippingCost;

        // Calcolo della percentuale di sconto
        var discountRatio = 1m - (netRevenue / grossTotal);

        // Arrotondamento
        var decDiscountPct = Math.Round(discountRatio * 100m, 2, MidpointRounding.AwayFromZero);

        var calculatedDiscountPct = (int)decDiscountPct;

        var msg = string.Format("DiscountPct modificato in {0} valore originale {1}", calculatedDiscountPct, line.DiscountPct);
        context.Messages.Add(msg);
        _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

        line.DiscountPct = calculatedDiscountPct;
    }
}