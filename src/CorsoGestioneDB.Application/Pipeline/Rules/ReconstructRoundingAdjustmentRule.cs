using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructRoundingAdjustmentRule : IReconstructionRule
{
    private readonly ILogger<ReconstructRoundingAdjustmentRule> _logger;

    /// <summary>
    /// Regola applicata qualora tutti i dati sembrino validi ma ci sia un
    /// minimo scostamento tra Revenue e il dato calcolato. Gli altri record
    /// vengono scartati
    /// </summary>
    public ReconstructRoundingAdjustmentRule(ILogger<ReconstructRoundingAdjustmentRule> logger)
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

        var roundingAdj = calculatedRevenue - revenue;

        if (roundingAdj != 0m)
        {
            // Verifica la presenza di uno scostamento tra il dato calcolato e il dato in arrivo
            if (Math.Abs(roundingAdj) < 0.02m)
            {
                var msg = string.Format("RoundingAdj modificato in {0} valore originale {1}", roundingAdj, line.RoundingAdj);
                context.Messages.Add(msg);
                _logger.LogInformation("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);

                line.RoundingAdj = roundingAdj;
            }
            else
            {
                var msg = string.Format("RoundingAdj {0} valore originale {1}. I dati dell'ordine non sono coerenti e non possono essere importati nel database", roundingAdj, line.RoundingAdj);
                context.Messages.Add(msg);
                _logger.LogError("Ordine: {0} campo {1}", context.Data.Order.OrderID, msg);
                context.MarkAsRejected(msg);

                line.RoundingAdj = roundingAdj;
            }            
        }
    }
}