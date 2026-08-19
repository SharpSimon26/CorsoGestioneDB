namespace CorsoGestioneDB.Domain.Entities;

public class Order
{
    public string OrderID { get; set; } = string.Empty;
    public DateTime? OrderDate { get; set; }
    public int CustomerID { get; set; }
    public int PaymentMethodID { get; set; }
    public int SalesChannelID { get; set; }
    public int OrderStatusID { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
