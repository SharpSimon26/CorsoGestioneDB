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
        var paymentMethodName = order.PaymentMethod!;

        // Tentativo di risoluzione esatta
        var paymentMethod = await _paymentMethodRepository.GetByNameAsync(paymentMethodName);

        // Tentativo di risoluzione approssimata
        paymentMethod ??= await _paymentMethodRepository.GetBestMatchByNameAsync(paymentMethodName);

        if (paymentMethod == null)
        {
            context.AddIssue(nameof(order.PaymentMethod), $"Metodo di pagamento '{order.PaymentMethod}' non riconosciuto.");
            return;
        }

        context.AddModification(nameof(order.PaymentMethodID), paymentMethod.PaymentMethodID, order.PaymentMethodID, "Database lookup", Stage.RESOLVE);
        order.PaymentMethodID = paymentMethod.PaymentMethodID;
    }
}