using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

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
               line.Quantity.HasValue && line.Quantity <= 0 &&
               line.UnitPrice.HasValue && line.UnitPrice > 0 &&
               line.DiscountPct.HasValue && line.DiscountPct >= 0;
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