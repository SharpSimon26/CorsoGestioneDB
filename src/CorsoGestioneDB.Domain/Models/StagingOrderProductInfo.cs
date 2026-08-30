namespace CorsoGestioneDB.Domain.Models;

public class StagingOrderProductInfo
{
    public required string ProductCode { get; set; }
    public required string ProductName { get; set; }
    public int NumOrders { get; set; }
    public decimal UsagePercentage { get; set; }
}