using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Helpers;
using Microsoft.Extensions.Logging;

namespace CorsoGestioneDB.Application.Pipeline;

public class ConvertStage : StageBase
{
    public ConvertStage(ILogger<ConvertStage> logger) : base(logger)
    {
    }

    public override async Task ExecuteAsync(IEnumerable<ImportContext> contexts)
    {
        foreach (var context in contexts.Where(x => x.IsProcessable()))
        {
            ConvertProduct(context);
            ConvertCustomer(context);
            ConvertOrder(context);
            ConvertOrderLine(context);
        }
    }

    public void ConvertOrder(ImportContext context)
    {
        // OrderID
        context.Data.Order.OrderID = context.RawOrder.OrderID;

        // OrderDate
        var dtOrderDate = ConvertHelper.ToDateTime(context.RawOrder.OrderDate);
    
        if (dtOrderDate.Success)
        {
            context.Data.Order.OrderDate = dtOrderDate.Value;
        }
        else
        {
            context.Messages.Add(dtOrderDate.ErrorMessage);
            logger.LogInformation(dtOrderDate.ErrorMessage);
        }
        
        // PaymentMethod
        context.Data.Order.PaymentMethod = context.RawOrder.PaymentMethod;

        // SalesChannel
        context.Data.Order.SalesChannel = context.RawOrder.SalesChannel;

        // OrderStatus
        context.Data.Order.OrderStatus = context.RawOrder.OrderStatus;

        // DeliveryDate -> potrebbe essere null
        if (!string.IsNullOrWhiteSpace(context.RawOrder.DeliveryDate))
        {
            var dtDeliveryDate = ConvertHelper.ToDateTime(context.RawOrder.DeliveryDate);

            if (dtDeliveryDate.Success)
            {
                context.Data.Order.DeliveryDate = dtDeliveryDate.Value;
            }
            else
            {
                context.Messages.Add(dtDeliveryDate.ErrorMessage);
                logger.LogInformation(dtDeliveryDate.ErrorMessage);
            }
        }
    }

    public void ConvertOrderLine(ImportContext context)
    {
        // Quantity
        var qty = ConvertHelper.ToInt(context.RawOrder.Quantity);

        if (qty.Success)
        {
            context.Data.OrderLine.Quantity = qty.Value;
        }
        else
        {
            context.Messages.Add(qty.ErrorMessage);
            logger.LogInformation(qty.ErrorMessage);
        }

        // UnitPrice
        var unitPrice = ConvertHelper.ToDecimal(context.RawOrder.UnitPrice);

        if (unitPrice.Success)
        {
            context.Data.OrderLine.UnitPrice = unitPrice.Value;
        }
        else
        {
            context.Messages.Add(unitPrice.ErrorMessage);
            logger.LogInformation(unitPrice.ErrorMessage);
        }

        // DiscountPct
        var discountPct = ConvertHelper.ToInt(context.RawOrder.DiscountPct);

        if (discountPct.Success)
        {
            context.Data.OrderLine.DiscountPct = discountPct.Value;
        }
        else
        {
            context.Messages.Add(discountPct.ErrorMessage);
            logger.LogInformation(discountPct.ErrorMessage);
        }

        // ShippingCost
        var shippingCost = ConvertHelper.ToDecimal(context.RawOrder.ShippingCost);

        if (shippingCost.Success)
        {
            context.Data.OrderLine.ShippingCost = shippingCost.Value;
        }
        else
        {
            context.Messages.Add(shippingCost.ErrorMessage);
            logger.LogInformation(shippingCost.ErrorMessage);
        }

        // Revenue
        var revenue = ConvertHelper.ToDecimal(context.RawOrder.Revenue);

        if (revenue.Success)
        {
            context.Data.OrderLine.Revenue = revenue.Value;
        }
        else
        {
            context.Messages.Add(revenue.ErrorMessage);
            logger.LogInformation(revenue.ErrorMessage);
        }
    }

    public void ConvertCustomer(ImportContext context)
    {
        // CustomerID
        var customerId = ConvertHelper.ToInt(context.RawOrder.CustomerID);

        if (customerId.Success)
        {
            context.Data.Customer.CustomerID = customerId.Value;
        }
        else
        {
            context.Messages.Add(customerId.ErrorMessage);
            logger.LogInformation(customerId.ErrorMessage);
        }

        // FirstName
        context.Data.Customer.FirstName = context.RawOrder.FirstName;

        // LastName
        context.Data.Customer.LastName = context.RawOrder.LastName;

        // Email
        context.Data.Customer.Email = context.RawOrder.Email;

        // City
        context.Data.Customer.City = context.RawOrder.City;

        // Province
        context.Data.Customer.Province = context.RawOrder.Province;

        // Region
        context.Data.Customer.Region = context.RawOrder.Region;

        // SignupDate
        var signupDate = ConvertHelper.ToDateTime(context.RawOrder.SignupDate);

        if (signupDate.Success)
        {
            context.Data.Customer.SignupDate = signupDate.Value;
        }
        else
        {
            context.Messages.Add(signupDate.ErrorMessage);
            logger.LogInformation(signupDate.ErrorMessage);
        }
    }

    public static void ConvertProduct(ImportContext context)
    {
        // ProductCode
        context.Data.Product.ProductCode = context.RawOrder.ProductCode;

        // ProductName
        context.Data.Product.ProductName = context.RawOrder.ProductName;

        // Category
        context.Data.Product.CategoryName = context.RawOrder.Category;
    }
}