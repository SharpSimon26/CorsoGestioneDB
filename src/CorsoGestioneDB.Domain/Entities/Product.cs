namespace CorsoGestioneDB.Domain.Entities;

public class Product
{
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int CategoryID { get; set; }
}
