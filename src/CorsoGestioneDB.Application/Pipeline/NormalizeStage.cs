using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Helpers;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class NormalizeStage : StageBase
{
    private const string NORM_LOG_MESSAGE = "{0} Campo {1} normalizzato. Valore originale: \"{2}\". Valore normalizzato: \"{3}\".";

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
            context.RawOrder.OrderID = orderId.Value;
            var msg = string.Format(NORM_LOG_MESSAGE, orderId.Value, "OrderID", orderId.OriginalValue, orderId.Value);
            context.Messages.Add(msg);
            logger.LogInformation(msg);
        }

        // OrderDate

        // PaymentMethod
        var paymentMethod = TextHelper.Normalize(context.RawOrder.PaymentMethod);

        if (paymentMethod.Changed)
        {
            context.RawOrder.PaymentMethod = paymentMethod.Value;
            var msg = string.Format(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "PaymentMethod", paymentMethod.OriginalValue, paymentMethod.Value);
            context.Messages.Add(msg);
            logger.LogInformation(msg);
        }

        // SalesChannel
        var salesChannel = TextHelper.Normalize(context.RawOrder.SalesChannel);

        if (salesChannel.Changed)
        {
            context.RawOrder.SalesChannel = salesChannel.Value;
            var msg = string.Format(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "SalesChannel", salesChannel.OriginalValue, salesChannel.Value);
            context.Messages.Add(msg);
            logger.LogInformation(msg);
        }

        // OrderStatus
        var orderStatus = TextHelper.Normalize(context.RawOrder.OrderStatus);

        if (orderStatus.Changed)
        {
            context.RawOrder.OrderStatus = orderStatus.Value;
            var msg = string.Format(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "OrderStatus", orderStatus.OriginalValue, orderStatus.Value);
            context.Messages.Add(msg);
            logger.LogInformation(msg);
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
            context.RawOrder.FirstName = firstName.Value;
            var msg = string.Format(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "FirstName", firstName.OriginalValue, firstName.Value);
            context.Messages.Add(msg);
            logger.LogInformation(msg);
        }

        // LastName
        var lastName = TextHelper.Normalize(context.RawOrder.LastName);

        if (lastName.Changed)
        {
            context.RawOrder.LastName = lastName.Value;
            var msg = string.Format(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "LastName", lastName.OriginalValue, lastName.Value);
            context.Messages.Add(msg);
            logger.LogInformation(msg);
        }

        // Email
        var email = EmailHelper.Normalize(context.RawOrder.Email);

        if (email.Changed)
        {
            context.RawOrder.Email = email.Value;
            var msg = string.Format(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "Email", email.OriginalValue, email.Value);
            context.Messages.Add(msg);
            logger.LogInformation(msg);
        }

        // Phone
        var phone = TextHelper.Normalize(context.RawOrder.Phone);

        if (phone.Changed)
        {
            context.RawOrder.Phone = phone.Value;
            logger.LogInformation(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "Phone", phone.OriginalValue, phone.Value);
        }

        // City
        var city = TextHelper.Normalize(context.RawOrder.City);

        if (city.Changed)
        {
            context.RawOrder.City = city.Value;
            logger.LogInformation(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "City", city.OriginalValue, city.Value);
        }

        // Province
        var province = TextHelper.Normalize(context.RawOrder.Province);

        if (province.Changed)
        {
            context.RawOrder.Province = province.Value;
            logger.LogInformation(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "Province", province.OriginalValue, province.Value);
        }

        // Region
        var region = TextHelper.Normalize(context.RawOrder.Region);

        if (region.Changed)
        {
            context.RawOrder.Region = region.Value;
            logger.LogInformation(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "Region", region.OriginalValue, region.Value);
        }

        // SignupDate
    }

    private void NormalizeProduct(ImportContext context)
    {
        // ProductCode
        var productCode = TextHelper.Normalize(context.RawOrder.ProductCode);

        if (productCode.Changed)
        {
            context.RawOrder.ProductCode = productCode.Value;
            logger.LogInformation(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "ProductCode", productCode.OriginalValue, productCode.Value);
        }

        // ProductNamw
        var productName = TextHelper.Normalize(context.RawOrder.ProductName);

        if (productName.Changed)
        {
            context.RawOrder.ProductName = productName.Value;
            logger.LogInformation(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "ProductName", productName.OriginalValue, productName.Value);
        }

        // Category
        var category = TextHelper.Normalize(context.RawOrder.Category);

        if (category.Changed)
        {
            context.RawOrder.Category = category.Value;
            logger.LogInformation(NORM_LOG_MESSAGE, context.RawOrder.OrderID, "Category", category.OriginalValue, category.Value);
        }
    }
}
