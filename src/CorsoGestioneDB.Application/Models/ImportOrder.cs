namespace CorsoGestioneDB.Application.Models;

public class ImportOrder
{
    public string? OrderID { get; set; }
    public DateTime? OrderDate { get; set; }
    public int? PaymentMethodID { get; set; }
    public string? PaymentMethod { get; set; }
    public int? SalesChannelID { get; set; }
    public string? SalesChannel { get; set; }
    public int? OrderStatusID { get; set; }
    public string? OrderStatus { get; set; }
    public DateTime? DeliveryDate { get; set; }
}