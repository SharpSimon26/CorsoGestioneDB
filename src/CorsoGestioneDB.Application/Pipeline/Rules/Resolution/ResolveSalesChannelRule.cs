using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Resolution;

public class ResolveSalesChannelRule : IResolutionRule
{
    private readonly ICachedSalesChannelRepository _salesChannelRepository;

    /// <summary>
    /// Regola di risoluzione applicata a SalesChannelID
    /// </summary>
    public ResolveSalesChannelRule(ICachedSalesChannelRepository salesChannelRepository)
    {
        _salesChannelRepository = salesChannelRepository;
    }

    public bool CanApply(ImportContext context)
    {
        var order = context.Data.Order;

        return !string.IsNullOrWhiteSpace(order.SalesChannel) &&
               order.SalesChannelID == null;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var order = context.Data.Order;

        // Recupera il canale di vendita dal database
        var channel = await _salesChannelRepository.GetByNameAsync(order.SalesChannel!);

        if (channel != null)
        {
            context.AddModification(nameof(order.SalesChannelID), channel.SalesChannelID, order.SalesChannelID, "Database lookup", Stage.RESOLVE);
            order.SalesChannelID = channel.SalesChannelID;
        }
        else
        {
            context.AddIssue(nameof(order.SalesChannel), $"Canale vendita '{order.SalesChannel}' non trovato.");
        }
    }
}
