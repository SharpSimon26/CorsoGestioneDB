using CorsoGestioneDB.Application.Engine;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class DuplicateStage : StageBase
{
    public DuplicateStage(ILogger<DuplicateStage> logger) : base(logger)
    {
    }

    public override async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        // Estrae gli OrderID duplicati da RawOrders
        var groups = contexts.Where(x => x.IsProcessable())
                             .GroupBy(x => x.RawOrder.OrderID)
                             .Where(y => y.Count() > 1);

        // Ciclo per i gruppi con OrderID duplicato
        foreach (var group in groups)
        {
            // Prende il primo elemento del gruppo e lo confronta con gli altri
            var firstItem = group.First();
            var duplicates = group.Skip(1).ToList();

            // Se sono identici segna gli altri come duplicati
            foreach (var item in duplicates)
            {
                if (IsDuplicate(item, firstItem))
                {
                    item.MarkAsDuplicate();
                    logger.LogInformation("Rilevato Ordine {0} duplicato", item.RawOrder.OrderID);
                }
                else
                {
                    item.MarkAsConflict();
                    logger.LogInformation("Rilevato Ordine {0} in conflitto", item.RawOrder.OrderID);
                }
            }
        }
    }

    private static bool IsDuplicate(ImportContext item, ImportContext firstItem)
    {
        return item.RawOrder.OrderID     == firstItem.RawOrder.OrderID     &&
               item.RawOrder.OrderDate   == firstItem.RawOrder.OrderDate   &&
               item.RawOrder.CustomerID  == firstItem.RawOrder.CustomerID  &&
               item.RawOrder.FirstName   == firstItem.RawOrder.FirstName   &&
               item.RawOrder.LastName    == firstItem.RawOrder.LastName    &&
               item.RawOrder.Email       == firstItem.RawOrder.Email       &&
               item.RawOrder.City        == firstItem.RawOrder.City        &&
               item.RawOrder.ProductCode == firstItem.RawOrder.ProductCode &&
               item.RawOrder.ProductName == firstItem.RawOrder.ProductName &&
               item.RawOrder.Quantity    == firstItem.RawOrder.Quantity    &&
               item.RawOrder.UnitPrice   == firstItem.RawOrder.UnitPrice   &&
               item.RawOrder.DiscountPct == firstItem.RawOrder.DiscountPct &&
               item.RawOrder.OrderStatus == firstItem.RawOrder.OrderStatus &&
               item.RawOrder.Revenue     == firstItem.RawOrder.Revenue;
    }
}