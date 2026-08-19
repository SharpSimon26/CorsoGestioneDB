namespace CorsoGestioneDB.Domain.Entities;

public class OrderLine
{
    public int OrderLineID { get; set; }
    public string OrderID { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int DiscountPct { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Revenue { get; set; }
}
