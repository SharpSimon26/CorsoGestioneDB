using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Resolution;

public class ResolvePaymentMethodRule : IResolutionRule
{
    private readonly ICachedPaymentMethodRepository _paymentMethodRepository;

    /// <summary>
    /// Regola di risoluzione applicata a PaymentMethodID
    /// </summary>
    public ResolvePaymentMethodRule(ICachedPaymentMethodRepository paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
    }

    public bool CanApply(ImportContext context)
    {
        var order = context.Data.Order;

        return !string.IsNullOrWhiteSpace(order.PaymentMethod) &&
               order.PaymentMethodID == null;
    }    

    public async Task ApplyAsync(ImportContext context)
    {
        var order = context.Data.Order;

        // Recupera lo stato dell'ordine dal database
        var method = await _paymentMethodRepository.GetByNameAsync(order.PaymentMethod!);

        if (method != null)
        {
            context.AddModification(nameof(order.PaymentMethodID), method.PaymentMethodID, order.PaymentMethodID, "Database lookup", Stage.RESOLVE);
            order.PaymentMethodID = method.PaymentMethodID;
        }
        else
        {
            context.AddIssue(nameof(order.PaymentMethod), $"Stato '{order.PaymentMethod}' non trovato.");
        }
    }
}