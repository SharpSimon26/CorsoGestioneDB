using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

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

        return line.Revenue.HasValue && line.Revenue > 0 &&
               line.Quantity.HasValue && line.Quantity > 0 &&
               line.UnitPrice.HasValue && line.UnitPrice <= 0 &&
               line.DiscountPct.HasValue && line.DiscountPct >= 0 &&
               line.ShippingCost.HasValue && line.ShippingCost >=0;
    }
    
    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var revenue = line.Revenue.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();
        var quantity = line.Quantity.GetValueOrDefault();
        var discountPct = line.DiscountPct.GetValueOrDefault();
        var discountFactor = quantity * (1 - (discountPct / 100m)); // quantità di articoli se comprati a prezzo pieno

        var calculatedUnitPrice = Math.Round(
            (revenue - shippingCost) / discountFactor, 2, MidpointRounding.AwayFromZero
        );

        var msg = string.Format("UnitPrice modificato in {0} valore originale {1}", calculatedUnitPrice, line.UnitPrice);
        context.Messages.Add(msg);
        _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

        line.UnitPrice = calculatedUnitPrice;
    }
}