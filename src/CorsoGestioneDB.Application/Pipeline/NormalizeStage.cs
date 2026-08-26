using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Helpers;
using CorsoGestioneDB.Application.Models;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class NormalizeStage : StageBase
{
    public NormalizeStage(ILogger<NormalizeStage> logger) : base(logger)
    {
    }

    public override async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        foreach (var context in contexts)
        {
            NormalizeOrder(context);
            // NormalizeOrderLine(context);
            NormalizeCustomer(context);
            NormalizeProduct(context);
        }
    }

    private void NormalizeOrder(ImportContext context)
    {
        // OrderID
        var orderId = TextHelper.Normalize(context.RawOrder.OrderID);

        if (orderId.Changed)
        {
            context.AddModification("OrderID", orderId.Value, orderId.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.OrderID = orderId.Value;
        }

        // OrderDate

        // PaymentMethod
        var paymentMethod = TextHelper.Normalize(context.RawOrder.PaymentMethod);

        if (paymentMethod.Changed)
        {
            context.AddModification("PaymentMethod", paymentMethod.Value, paymentMethod.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.PaymentMethod = paymentMethod.Value;
        }

        // SalesChannel
        var salesChannel = TextHelper.Normalize(context.RawOrder.SalesChannel);

        if (salesChannel.Changed)
        {
            context.AddModification("SalesChannel", salesChannel.Value, salesChannel.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.SalesChannel = salesChannel.Value;
        }

        // OrderStatus
        var orderStatus = TextHelper.Normalize(context.RawOrder.OrderStatus);

        if (orderStatus.Changed)
        {
            context.AddModification("OrderStatus", orderStatus.Value, orderStatus.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.OrderStatus = orderStatus.Value;
        }

        // DeliveryDate
    }

    private static void NormalizeOrderLine(ImportContext context)
    {
        // Quantity
        // UnitPrice
        // DiscountPct
        // ShippingCost
        // Revenue
    }

    private void NormalizeCustomer(ImportContext context)
    {
        // CustomerID

        // FirstName
        var firstName = TextHelper.Normalize(context.RawOrder.FirstName);

        if (firstName.Changed)
        {
            context.AddModification("FirstName", firstName.Value, firstName.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.FirstName = firstName.Value;
        }

        // LastName
        var lastName = TextHelper.Normalize(context.RawOrder.LastName);

        if (lastName.Changed)
        {
            context.AddModification("LastName", lastName.Value, lastName.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.LastName = lastName.Value;
        }

        // Email
        var email = EmailHelper.Normalize(context.RawOrder.Email);

        if (email.Changed)
        {
            context.AddModification("Email", email.Value, email.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.Email = email.Value;
        }

        // Phone
        var phone = TextHelper.Normalize(context.RawOrder.Phone);

        if (phone.Changed)
        {
            context.AddModification("Phone", phone.Value, phone.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.Phone = phone.Value;
        }

        // City
        var city = TextHelper.Normalize(context.RawOrder.City);

        if (city.Changed)
        {
            context.AddModification("City", city.Value, city.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.City = city.Value;
        }

        // Province
        var province = TextHelper.Normalize(context.RawOrder.Province);

        if (province.Changed)
        {
            context.AddModification("Province", province.Value, province.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.Province = province.Value;
        }

        // Region
        var region = TextHelper.Normalize(context.RawOrder.Region);

        if (region.Changed)
        {
            context.AddModification("Region", region.Value, region.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.Region = region.Value;
        }

        // SignupDate
    }

    private void NormalizeProduct(ImportContext context)
    {
        // ProductCode
        var productCode = TextHelper.Normalize(context.RawOrder.ProductCode);

        if (productCode.Changed)
        {
            context.AddModification("ProductCode", productCode.Value, productCode.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.ProductCode = productCode.Value;
        }

        // ProductNamw
        var productName = TextHelper.Normalize(context.RawOrder.ProductName);

        if (productName.Changed)
        {
            context.AddModification("ProductName", productName.Value, productName.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.ProductName = productName.Value;
        }

        // Category
        var category = TextHelper.Normalize(context.RawOrder.Category);

        if (category.Changed)
        {
            context.AddModification("Category", category.Value, category.OriginalValue, GetType().Name, Stage.NORMALIZE);
            context.RawOrder.Category = category.Value;
        }
    }
}
