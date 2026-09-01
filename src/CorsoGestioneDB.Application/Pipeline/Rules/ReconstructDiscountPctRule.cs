using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ReconstructDiscountPctRule : IReconstructionRule
{
    /// <summary>
    /// Regola di ricostruzione applicata qualora gli altri dati appaiano coerenti 
    /// ma la percentuale di sconto sia minore di 0 o maggiore di 99
    /// </summary>
    public ReconstructDiscountPctRule()
    {
    }

    public bool CanApply(ImportContext context)
    {
        var line = context.Data.OrderLine;

        return line.Quantity.HasValue && line.Quantity > 0 &&
               line.UnitPrice.HasValue && line.UnitPrice > 0 &&
               (line.DiscountPct == null || line.DiscountPct < 0 || line.DiscountPct > 99) &&
               line.ShippingCost.HasValue && line.ShippingCost >= 0 &&
               line.Revenue.HasValue && line.Revenue > 0;        
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var line = context.Data.OrderLine;
        var quantity = line.Quantity.GetValueOrDefault();
        var unitPrice = line.UnitPrice.GetValueOrDefault();
        var shippingCost = line.ShippingCost.GetValueOrDefault();
        var revenue = line.Revenue.GetValueOrDefault();

        // Prezzo totale non scontato della merce
        var grossTotal = quantity * unitPrice;

        // Incasso senza spese di spedizione
        var netRevenue = revenue - shippingCost;

        // Calcolo della percentuale di sconto
        var discountRatio = 1m - (netRevenue / grossTotal);

        // Arrotondamento e trasformazione in int
        var decDiscountPct = Math.Round(discountRatio * 100m, 2, MidpointRounding.AwayFromZero);
        var calculatedDiscountPct = (int)decDiscountPct;

        // Traccia della modifica
        context.AddModification("DiscountPct", calculatedDiscountPct, line.DiscountPct, GetType().Name, Stage.RECONSTRUCT);

        // Dato corretto
        line.DiscountPct = calculatedDiscountPct;
    }
}